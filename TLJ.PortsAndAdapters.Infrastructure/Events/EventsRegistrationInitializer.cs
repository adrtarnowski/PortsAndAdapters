using System.Threading.Tasks;
using Kitbag.Builder.Core.Initializer;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using TLJ.PortsAndAdapters.Application.Bookmaking.Events;

namespace TLJ.PortsAndAdapters.Infrastructure.Events
{
    public class EventsRegistrationInitializer : IInitializer
    {
        private readonly IEventSubscriber _eventSubscriber;

        public EventsRegistrationInitializer(IEventSubscriber eventSubscriber)
        {
            _eventSubscriber = eventSubscriber;
        }

        public async Task InitializeAsync()
        {
            await _eventSubscriber.RemoveDefaultRule();
            await _eventSubscriber.AddCustomRule(Kitbag.Builder.MessageBus.Extensions.GetEventFor<CloseBookmakingEvent>());
        }
    }
}