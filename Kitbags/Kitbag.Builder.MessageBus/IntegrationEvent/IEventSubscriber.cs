using System.Threading.Tasks;

namespace Kitbag.Builder.MessageBus.IntegrationEvent;

public interface IEventSubscriber
{
    Task RegisterOnMessageHandlerAndReceiveMessages();
    void Subscribe<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>;
    Task CloseSubscriptionAsync();
    ValueTask DisposeAsync();
}