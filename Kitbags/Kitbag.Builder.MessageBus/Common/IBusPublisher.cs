using System.Threading.Tasks;

namespace Kitbag.Builder.MessageBus.Common
{
    public interface IBusPublisher
    {
        Task PublishEventAsync(object payload, string type, string? sessionId);
    }
}