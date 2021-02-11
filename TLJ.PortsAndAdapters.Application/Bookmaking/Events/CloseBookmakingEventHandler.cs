using System.Threading.Tasks;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Microsoft.Extensions.Logging;

namespace TLJ.PortsAndAdapters.Application.Bookmaking.Events
{
    public class CloseBookmakingEventHandler : IIntegrationEventHandler<CloseBookmakingEvent>
    {
        private readonly ILogger<CloseBookmakingEventHandler> _logger;

        public CloseBookmakingEventHandler(ILogger<CloseBookmakingEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(CloseBookmakingEvent integrationEvent)
        {
            _logger.Log(LogLevel.Information,"Reaction in this event");
        }
    }
}