using System.Threading.Tasks;

namespace Kitbag.Builder.MessageBus.IntegrationEvent
{
    public interface IEventSubscriber
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();
        void Subscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;
        Task AddCustomRule(string subject);
        Task RemoveDefaultRule();
        Task CloseSubscriptionAsync();
        ValueTask DisposeAsync();
    }
}