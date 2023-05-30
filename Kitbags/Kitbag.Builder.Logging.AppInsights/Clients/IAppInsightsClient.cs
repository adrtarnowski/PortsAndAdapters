namespace Kitbag.Builder.Logging.AppInsights.Clients
{
    public interface IAppInsightsClient
    {
        public void TrackEvent(IAppInsightsEvent appInsightsEvent, string userId, string correlationId);
        public void TrackMetric(IAppInsightsMetric appInsightsMetric, double value);
        public void TrackMetric(IAppInsightsMetric appInsightsMetric, double value, string userId);
        public void TrackMetricWithDimension(IAppInsightsMetric appInsightsMetric, double value, string dimensionName, string dimensionValue);
        public void TrackMetricWithDimension(IAppInsightsMetric appInsightsMetric, double value, string dimensionName, string dimensionValue, string userId);
    }
}