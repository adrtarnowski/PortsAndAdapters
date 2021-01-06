namespace Kitbag.Builder.Logging.AppInsights.Common
{
    public class LoggingProperties
    {
        public Level? LogLevel { get; set; }
        public AppInsightsProperties? ApplicationInsights { get; set; }
        public string? File { get; set; }
    }

    public class AppInsightsProperties
    {
        public string? InstrumentationKey { get; set; }
    }

    public class Level
    {
        public string? Default { get; set; }
    }
}