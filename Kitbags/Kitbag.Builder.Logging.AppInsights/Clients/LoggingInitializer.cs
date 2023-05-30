using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Kitbag.Builder.Logging.AppInsights.Clients
{
    internal class LoggingInitializer : ITelemetryInitializer
    {
        private readonly string roleName;
        public LoggingInitializer(string? microserviceName)
        {
            this.roleName = microserviceName ?? throw new ArgumentException("Microservice name is not set for logging");
        }
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = roleName;
        }
    }
}