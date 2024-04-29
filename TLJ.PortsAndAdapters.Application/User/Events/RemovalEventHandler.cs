using System.Threading.Tasks;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Microsoft.Extensions.Logging;

namespace TLJ.PortsAndAdapters.Application.User.Events;

public class RemovalEventHandler : IIntegrationEventHandler<RemovalUserEvent>
{
    private readonly ILogger<RemovalEventHandler> _logger;

    public RemovalEventHandler(ILogger<RemovalEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(RemovalUserEvent integrationEvent)
    {
        _logger.Log(LogLevel.Information,"Reaction for this event");
    }
}