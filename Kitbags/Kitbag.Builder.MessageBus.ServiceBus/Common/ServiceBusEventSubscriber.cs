using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.CQRS.IntegrationEvents.Common;
using Kitbag.Builder.MessageBus.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common
{
    public class ServiceBusEventSubscriber : IEventBusSubscriber, IDisposable
    {
        private bool _disposed;
        private readonly BusProperties _busProperties;
        private readonly ILogger<ServiceBusEventSubscriber> _logger;
        private readonly IBusSubscriptionsManager _busSubscriptionManager;
        private readonly IIntegrationEventDispatcher _eventDispatcher;
        private ISubscriptionClient _subscriptionClient;

        private ISubscriptionClient Client => _subscriptionClient = _subscriptionClient.IsClosedOrClosing ? GetSubscriptionClient() : _subscriptionClient;

        public ServiceBusEventSubscriber(
            BusProperties busProperties,
            ILogger<ServiceBusEventSubscriber> logger,
            IBusSubscriptionsManager busSubscriptionsManager, 
            IIntegrationEventDispatcher eventDispatcher)
        {
            _busProperties = busProperties;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subscriptionClient = new SubscriptionClient(busProperties.ConnectionString, busProperties.EventTopicName, busProperties.EventSubscriptionName);
            _busSubscriptionManager = busSubscriptionsManager ?? throw new ArgumentNullException(nameof(busSubscriptionsManager));
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            RemoveDefaultRule();
            Client.RegisterSessionHandler(
                async (session, message, token) =>
                {
                    if (await ProcessMessage(message)) 
                        await session.CompleteAsync(message.SystemProperties.LockToken);
                }, 
                new SessionHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentSessions = _busProperties.MaxConcurrentCalls ?? 16,
                    AutoComplete = _busProperties.AutoComplete ?? false,
                    MessageWaitTimeout = TimeSpan.FromSeconds(_busProperties.MessageWaitTimeoutInSeconds ?? 1),
                    MaxAutoRenewDuration = TimeSpan.FromMinutes(_busProperties.MaxAutoRenewMinutesDuration ?? 5),
                });
        }

        public void Subscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _busSubscriptionManager.GetEventLabel<T>();
            if (!_busSubscriptionManager.HasSubscriptionsForEvent<T>()) 
                AddCustomRule(eventName);
            
            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);
            _busSubscriptionManager.AddSubscription<T, TH>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) 
                return;

            if (disposing)
            {
                if (!_subscriptionClient.IsClosedOrClosing) 
                    AsyncHelper.RunSync(() => _subscriptionClient.CloseAsync());
            }
            _disposed = true;
        }

        private async Task<bool> ProcessMessage(Message message)
        {
            var processed = false;
            var eventName = message.Label;
            if (_busSubscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                var messageAsString = Encoding.UTF8.GetString(message.Body);

                var eventType = _busSubscriptionManager.GetEventTypeByName(eventName);
                var integrationEvent = (IIntegrationEvent)JsonConvert.DeserializeObject(messageAsString, eventType);
                var subscriptions = _busSubscriptionManager.GetHandlersForEvent(eventName).ToList();
                foreach (var subscription in subscriptions)
                {
                    await _eventDispatcher.SendAsync(integrationEvent, subscription).ConfigureAwait(false);
                }

                processed = true;
            }

            return processed;
        }

        private void RemoveDefaultRule()
        {
            try
            {
                AsyncHelper.RunSync(() => _subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName));
            }
            catch (MessagingEntityNotFoundException)
            {
                _logger.LogInformation($"The messaging entity {RuleDescription.DefaultRuleName} is removed.");
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            _logger.LogError(
                exceptionReceivedEventArgs.Exception,
                $"Message handler encountered an exception {context.EntityPath} | {context.Action}");
            return Task.CompletedTask;
        }

        private void AddCustomRule(string label)
        {
            try
            {
                AsyncHelper.RunSync(() => GetSubscriptionClient().AddRuleAsync(new RuleDescription
                {
                    Filter = new CorrelationFilter { Label = label },
                    Name = label
                }));
            }
            catch (ServiceBusException exception)
            {
                _logger.LogInformation($"The messaging entity {label} already exists.", exception.Message);
            }
        }

        private ISubscriptionClient GetSubscriptionClient()
            => new SubscriptionClient(
                _busProperties.ConnectionString,
                _busProperties.EventTopicName,
                _busProperties.EventSubscriptionName);

        ~ServiceBusEventSubscriber()
        {
            Dispose(false);
        }
    }
}