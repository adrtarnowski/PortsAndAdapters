using System;
using System.Text;
using Azure.Storage.Queues;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.MessageBus.AzureQueue.Client;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Kitbag.Builder.MessageBus.AzureQueue.Common
{
    public class QueueEventSubscriber : IEventBusSubscriber
    {
        private readonly QueueClient _queueClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        
        public QueueEventSubscriber(IAzureQueueClient queueClient, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _queueClient = queueClient.GetClient();
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dispatcher = scope.ServiceProvider.GetService<ICommandDispatcher>();
            while (_queueClient.Exists())
            {
               var message =  _queueClient.ReceiveMessage().Value;
               var messageAsString = Encoding.UTF8.GetString(message.Body);
               dynamic command = JsonConvert.DeserializeObject(messageAsString);
               AsyncHelper.RunSync(() => dispatcher.SendAsync(command).ConfigureAwait(false));
               _queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
            }
        }

        public void Subscribe<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }
    }
}