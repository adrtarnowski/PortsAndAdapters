using System;
using Azure.Storage.Queues;
using Kitbag.Builder.MessageBus.Common;

namespace Kitbag.Builder.MessageBus.AzureQueue.Client
{
    public class AzureQueueClient : IAzureQueueClient
    {
        private readonly QueueClient _queueClient;

        public AzureQueueClient(BusProperties busProperties, QueueClient queueClient)
        {
            _queueClient = queueClient;
            var connectionString = busProperties.ConnectionString ?? throw new ArgumentNullException(nameof( busProperties.ConnectionString));
            var queueName = busProperties.QueueName ?? throw new ArgumentNullException(nameof(busProperties.QueueName));
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public QueueClient GetClient()
        {
            return _queueClient;
        }
    }
}