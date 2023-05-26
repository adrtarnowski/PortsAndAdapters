using System;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Logging.AppInsights.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace Kitbag.Builder.Logging.AppInsights
{
    public static class Extensions
    {
        public static IKitbagBuilder AddAppInsights(this IKitbagBuilder builder, string sectionName = "Logging")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            
            var loggingProperties = builder.GetSettings<LoggingProperties>(sectionName);
            builder.Services.AddSingleton(loggingProperties);
            if (!Enum.TryParse<LogLevel>(loggingProperties?.LogLevel?.Default, true, out var level))
                level = LogLevel.Information;

            builder.Services.AddLogging(loggerBuilder =>
            {
                string? key = loggingProperties?.ApplicationInsights?.InstrumentationKey;
                if (key == null)
                    throw new ArgumentException("Instrumentation Key is missing");
                loggerBuilder.AddApplicationInsights();
                loggerBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", level);
            });
            builder.Services.AddApplicationInsightsTelemetry();

            return builder;
        }
    }
}