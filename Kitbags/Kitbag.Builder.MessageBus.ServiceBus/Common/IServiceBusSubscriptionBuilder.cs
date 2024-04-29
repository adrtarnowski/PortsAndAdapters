using System.Threading.Tasks;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common;

public interface IServiceBusSubscriptionBuilder
{
    Task AddCustomRule(string subject);
    Task RemoveDefaultRule();
}