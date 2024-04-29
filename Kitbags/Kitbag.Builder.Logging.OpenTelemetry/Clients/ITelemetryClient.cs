namespace Kitbag.Builder.Logging.OpenTelemetry.Clients;

public interface ITelemetryClient
{
    void TrackEvent(ITelemetryEvent telemetryEvent, string userId, string correlationId);

    void TrackMetric(ITelemetryMetric metric, double value);

    void TrackMetric(ITelemetryMetric metric, double value, string userId);

    void TrackMetricWithDimension(
        ITelemetryMetric metric,
        double value,
        string dimensionName,
        string dimensionValue);

    void TrackMetricWithDimension(
        ITelemetryMetric metric,
        double value,
        string dimensionName,
        string dimensionValue,
        string userId);
}