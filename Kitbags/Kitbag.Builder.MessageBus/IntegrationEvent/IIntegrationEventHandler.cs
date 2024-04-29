using System.Threading.Tasks;

namespace Kitbag.Builder.MessageBus.IntegrationEvent;

public interface IIntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    Task HandleAsync(TIntegrationEvent integrationEvent);
}