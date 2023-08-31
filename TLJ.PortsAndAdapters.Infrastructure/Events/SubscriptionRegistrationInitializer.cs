using System.Threading.Tasks;
using Kitbag.Builder.Core.Initializer;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using TLJ.PortsAndAdapters.Application.Bookmaking.Events;

namespace TLJ.PortsAndAdapters.Infrastructure.Events
{
    public class SubscriptionRegistrationInitializer : IInitializer
    {
        private readonly IServiceBusSubscriptionBuilder _subscriptionBuilder;

        public SubscriptionRegistrationInitializer(IServiceBusSubscriptionBuilder subscriptionBuilder)
        {
            _subscriptionBuilder = subscriptionBuilder;
        }

        public async Task InitializeAsync()
        {
            await _subscriptionBuilder.RemoveDefaultRule();
            await _subscriptionBuilder.AddCustomRule(Kitbag.Builder.MessageBus.Extensions.GetEventFor<CloseBookmakingEvent>());
        }
    }
}