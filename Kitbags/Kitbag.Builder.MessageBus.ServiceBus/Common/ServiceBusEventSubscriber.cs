using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Kitbag.Builder.CQRS.IntegrationEvents.Common;
using Kitbag.Builder.MessageBus.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common
{
    public class ServiceBusEventSubscriber : IEventSubscriber
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly BusProperties _busProperties;
        private readonly IEventManager _eventManager;
        private readonly IIntegrationEventDispatcher _eventDispatcher;
        private readonly ILogger<ServiceBusEventSubscriber> _logger;
        private ServiceBusSessionProcessor? _processor = null;

        public ServiceBusEventSubscriber(
            BusProperties busProperties,
            ILogger<ServiceBusEventSubscriber> logger,
            ServiceBusClient serviceBusClient,
            IEventManager eventManager,
            IIntegrationEventDispatcher eventDispatcher)
        {
            _busProperties = busProperties;
            _serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            _processor = _serviceBusClient.CreateSessionProcessor(_busProperties.EventTopicName,
                _busProperties.EventSubscriptionName,
                new ServiceBusSessionProcessorOptions()
                {
                    MaxConcurrentSessions = _busProperties.MaxConcurrentCalls ?? 16,
                    AutoCompleteMessages = _busProperties.AutoComplete ?? false,
                    SessionIdleTimeout = TimeSpan.FromSeconds(_busProperties.MessageWaitTimeoutInSeconds ?? 1),
                    MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(_busProperties.MaxAutoRenewMinutesDuration ?? 5)
                });

            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync().ConfigureAwait(false);
        }

        private async Task MessageHandler(ProcessSessionMessageEventArgs args)
        {
            if (await ProcessMessage(args.Message)) ;
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            _logger.LogError(
                arg.Exception,
                $"Message handler encountered an exception | ErrorSource: {arg.ErrorSource} - EntityPath: {arg.EntityPath} - FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");
            return Task.CompletedTask;
        }
        
        private async Task<bool> ProcessMessage(ServiceBusReceivedMessage message)
        {
            var processed = false;
            var eventName = message.Subject;
            if (_eventManager.HasSubscriptionsForEvent(eventName))
            {
                var messageAsString = Encoding.UTF8.GetString(message.Body);

                var eventType = _eventManager.GetEventTypeByName(eventName);
                var integrationEvent = (IIntegrationEvent)JsonConvert.DeserializeObject(messageAsString, eventType)!;
                var subscriptions = _eventManager.GetHandlersForEvent(eventName).ToList();
                foreach (var subscription in subscriptions)
                {
                    await _eventDispatcher.SendAsync(integrationEvent, subscription).ConfigureAwait(false);
                }

                processed = true;
            }

            return processed;
        }
        
        public void Subscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _eventManager.AddSubscription<T, TH>();
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            await _serviceBusClient.DisposeAsync().ConfigureAwait(false);
        }

        public async Task CloseSubscriptionAsync()
        {
            await _processor!.CloseAsync().ConfigureAwait(false);
        }
    }
}