using System.Threading.Tasks;
using Azure.Storage.Queues;
using Kitbag.Builder.MessageBus.AzureQueue.Client;
using Kitbag.Builder.MessageBus.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Newtonsoft.Json;

namespace Kitbag.Builder.MessageBus.AzureQueue.Common
{
    public class QueuePublisher : IBusPublisher
    {
        private readonly QueueClient _queueClient;

        public QueuePublisher(IAzureQueueClient queueClient)
        {
            _queueClient = queueClient.GetClient();
        }

        public async Task PublishEventAsync<T>(T payload, string? sessionId = null) where T : IIntegrationEvent
        {
            object objPayload = payload;
            string data = payload is string ? (string) objPayload : JsonConvert.SerializeObject(objPayload);
            await _queueClient.SendMessageAsync(data);
        }
    }
}