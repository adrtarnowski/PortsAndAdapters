using System.Threading.Tasks;
using Kitbag.Builder.MessageBus.IntegrationEvent;

namespace Kitbag.Builder.MessageBus.Common;

public interface IBusPublisher<T>  where T : IIntegrationEvent
{
    Task PublishEventAsync(T payload, string? sessionId = null);
}