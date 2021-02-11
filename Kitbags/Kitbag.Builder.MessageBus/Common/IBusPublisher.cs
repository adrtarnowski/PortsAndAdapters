using System.Threading.Tasks;
using Kitbag.Builder.MessageBus.IntegrationEvent;

namespace Kitbag.Builder.MessageBus.Common
{
    public interface IBusPublisher
    {
        Task PublishEventAsync<T>(T payload, string? sessionId = null) where T : IIntegrationEvent;
    }
}