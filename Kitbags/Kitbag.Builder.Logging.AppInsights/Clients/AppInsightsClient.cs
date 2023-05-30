using System.Collections.Generic;
using Kitbag.Builder.Logging.AppInsights.Common;
using Microsoft.ApplicationInsights;

namespace Kitbag.Builder.Logging.AppInsights.Clients
{
public class AppInsightsClient : IAppInsightsClient
    {
        private TelemetryClient _telemetry;
        private LoggingProperties? _loggingProperties;

        public AppInsightsClient(TelemetryClient telemetry, LoggingProperties loggingProperties)
        {
            _telemetry = telemetry;
            _loggingProperties = loggingProperties;

            _telemetry.InstrumentationKey = _loggingProperties?.ApplicationInsights?.InstrumentationKey;
        }

        public void TrackEvent(IAppInsightsEvent appInsightsEvent, string userId, string correlationId)
        {
            var properties = new Dictionary<string, string>
            {
                [LoggingConstants.UserId] = userId,
                [LoggingConstants.CorrelationId] = correlationId
            };

            _telemetry.TrackEvent(appInsightsEvent.Name, properties);
            _telemetry.Flush();
        }

        public void TrackMetric(IAppInsightsMetric appInsightsMetric, double value)
        {
            _telemetry
                .GetMetric(appInsightsMetric.Name)
                .TrackValue(value);
            _telemetry.Flush();
        }

        public void TrackMetric(IAppInsightsMetric appInsightsMetric, double value, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TrackMetric(appInsightsMetric, value);
                return;
            }
            
            _telemetry
                .GetMetric(appInsightsMetric.Name, LoggingConstants.UserId)
                .TrackValue(value, userId);
            _telemetry.Flush();
        }

        public void TrackMetricWithDimension(IAppInsightsMetric appInsightsMetric, double value, string dimensionName, string dimensionValue)
        {
            if (string.IsNullOrEmpty(dimensionName) || string.IsNullOrEmpty(dimensionValue))
            {
                TrackMetric(appInsightsMetric, value);
                return;
            }

            _telemetry
                .GetMetric(appInsightsMetric.Name, dimensionName)
                .TrackValue(value, dimensionValue);
            _telemetry.Flush();
        }

        public void TrackMetricWithDimension(IAppInsightsMetric appInsightsMetric, double value, string dimensionName, string dimensionValue, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TrackMetricWithDimension(appInsightsMetric, value, dimensionName, dimensionValue);
                return;
            }

            if (string.IsNullOrEmpty(dimensionName) || string.IsNullOrEmpty(dimensionValue))
            {
                TrackMetric(appInsightsMetric, value, userId);
                return;
            }

            _telemetry
                .GetMetric(
                    appInsightsMetric.Name,
                    dimensionName,
                    LoggingConstants.UserId)
                .TrackValue(
                    value,
                    dimensionValue,
                    userId);
            _telemetry.Flush();
        }
    }
}