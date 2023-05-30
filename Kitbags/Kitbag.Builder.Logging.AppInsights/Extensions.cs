using System;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.Logging.AppInsights.Clients;
using Kitbag.Builder.Logging.AppInsights.Common;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace Kitbag.Builder.Logging.AppInsights
{
    public static class Extensions
    {
        public static IKitbagBuilder AddAppInsights(this IKitbagBuilder builder, string sectionName = "Logging", string appName = "App")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            
            var loggingProperties = builder.GetSettings<LoggingProperties>(sectionName);
            var appProperties = builder.GetSettings<AppProperties>(appName);
            builder.Services.AddSingleton(loggingProperties);
            if (!Enum.TryParse<LogLevel>(loggingProperties?.LogLevel?.Default, true, out var level))
                level = LogLevel.Information;

            builder.Services.AddSingleton<IAppInsightsClient, AppInsightsClient>();
            builder.Services.AddLogging(loggerBuilder =>
            {
                string? key = loggingProperties?.ApplicationInsights?.InstrumentationKey;
                if (key == null)
                    throw new ArgumentException("Instrumentation Key is missing");
                loggerBuilder.AddApplicationInsights(key);
                
                loggerBuilder.AddApplicationInsights(
                    (telemetryConfiguration) => { telemetryConfiguration.InstrumentationKey = key; } , (applicationInsightsOptions) => {  });
                loggerBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", level);
                
            });
            builder.Services.AddSingleton<ITelemetryInitializer>(
                new LoggingInitializer(appProperties.Name));
            builder.Services.AddApplicationInsightsTelemetry();

            return builder;
        }
    }
}