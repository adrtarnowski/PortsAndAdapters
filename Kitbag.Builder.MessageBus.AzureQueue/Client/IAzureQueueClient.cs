using Azure.Storage.Queues;

namespace Kitbag.Builder.MessageBus.AzureQueue.Client
{
    public interface IAzureQueueClient
    {
        QueueClient GetClient();
    }
}