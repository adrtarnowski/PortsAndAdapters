namespace Kitbag.Builder.Logging.OpenTelemetry.Settings;

public record LoggingProperties
{
    public Level LogLevel { get; set; } = default!;
        
    public AzureMonitorProperties AzureMonitor { get; set; } = default!;
}
    
public record Level
{
    public string Default { get; set; } = default!;
}