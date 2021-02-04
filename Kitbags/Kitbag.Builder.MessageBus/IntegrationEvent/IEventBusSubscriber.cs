namespace Kitbag.Builder.MessageBus.IntegrationEvent
{
    public interface IEventBusSubscriber
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        void Subscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}