namespace Kitbag.Builder.Logging.OpenTelemetry.Settings;

public record AzureMonitorProperties
{
    public string ConnectionString { get; set; } = default!;
}