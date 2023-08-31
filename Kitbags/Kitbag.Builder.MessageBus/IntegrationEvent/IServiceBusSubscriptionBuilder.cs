using System.Threading.Tasks;

namespace Kitbag.Builder.MessageBus.IntegrationEvent
{
    public interface IServiceBusSubscriptionBuilder
    {
        Task AddCustomRule(string subject);
        Task RemoveDefaultRule();
    }
}