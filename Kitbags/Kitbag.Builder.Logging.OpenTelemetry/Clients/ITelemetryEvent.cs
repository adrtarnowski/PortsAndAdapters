namespace Kitbag.Builder.Logging.OpenTelemetry.Clients;

public interface ITelemetryEvent
{
    string Name { get; }
}