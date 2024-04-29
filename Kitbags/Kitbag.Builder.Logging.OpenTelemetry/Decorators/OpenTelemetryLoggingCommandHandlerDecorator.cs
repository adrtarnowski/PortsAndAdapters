using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.Logging.OpenTelemetry.Clients;
using Kitbag.Builder.Logging.OpenTelemetry.Common;
using Kitbag.Builder.RunningContext.Common;
using Microsoft.Extensions.Logging;

namespace Kitbag.Builder.Logging.OpenTelemetry.Decorators;

public class OpenTelemetryLoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ILogger<LoggerCommandHandlerDecorator> _logger;
    private readonly ICommandHandler<TCommand> _decoratedHandler;
    private readonly IRunningContextProvider _context;
    private readonly ITelemetryClient _telemetryClient;

    public OpenTelemetryLoggingCommandHandlerDecorator(ICommandHandler<TCommand> decoratedHandler,
        ILogger<LoggerCommandHandlerDecorator> logger,
        ITelemetryClient telemetryClient,
        IRunningContextProvider context)
    {
        _decoratedHandler = decoratedHandler;
        _logger = logger;
        _telemetryClient = telemetryClient;
        _context = context;
    }

    public async Task Handle(TCommand operation)
    {
        var correlationId = _context?.CorrelationId.ToString() ?? string.Empty;
        var userId = _context?.UserId ?? string.Empty;
        var requestId = _context?.RequestId.ToString() ?? string.Empty;

        _telemetryClient.TrackMetricWithDimension(
            CommandHandlerMetric.CommandRequested,
            1,
            CommandHandlerMetric.CommandNameDimension,
            operation.GetType().Name,
            userId);

        using (_logger.BeginScopeWith(userId, correlationId, new { RequestId = requestId }))
        {
            _logger.LogInformation($"Received command : {operation.GetType().Name}:{correlationId}");
            await _decoratedHandler.Handle(operation).ConfigureAwait(false);
            _logger.LogInformation($"Finished processing command : {operation.GetType().Name}:{correlationId}");
        }
    }

    public class LoggerCommandHandlerDecorator
    {
    }

    private class CommandHandlerMetric : ITelemetryMetric
    {
        public string Name { get; }
        public const string CommandNameDimension = "CommandName";

        private CommandHandlerMetric(string name)
        {
            Name = name;
        }

        public static CommandHandlerMetric CommandRequested => new CommandHandlerMetric(nameof(CommandRequested));
    }
}