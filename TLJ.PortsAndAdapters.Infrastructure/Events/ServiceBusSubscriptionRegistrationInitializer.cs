using System.Threading.Tasks;
using Kitbag.Builder.Core.Initializer;
using Kitbag.Builder.MessageBus.ServiceBus.Common;
using TLJ.PortsAndAdapters.Application.User.Events;

namespace TLJ.PortsAndAdapters.Infrastructure.Events
{
    public class ServiceBusSubscriptionRegistrationInitializer : IInitializer
    {
        private readonly IServiceBusSubscriptionBuilder _subscriptionBuilder;

        public ServiceBusSubscriptionRegistrationInitializer(IServiceBusSubscriptionBuilder subscriptionBuilder)
        {
            _subscriptionBuilder = subscriptionBuilder;
        }

        public async Task InitializeAsync()
        {
            await _subscriptionBuilder.RemoveDefaultRule();
            await _subscriptionBuilder.AddCustomRule(Kitbag.Builder.MessageBus.Extensions.GetEventFor<RemovalUserEvent>());
        }
    }
}