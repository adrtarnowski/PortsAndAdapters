using System.Diagnostics;
using System.Diagnostics.Metrics;
using Kitbag.Builder.Core.Common;

namespace Kitbag.Builder.Logging.OpenTelemetry.Clients;

public class TelemetryClient : IDisposable, ITelemetryClient
{
    private readonly TelemetrySource _telemetrySource;
    private readonly IMeterFactory _meterFactory;
    private readonly string _meterName;
    private Activity? _activity;

    public TelemetryClient(
        TelemetrySource telemetrySource,
        IMeterFactory meterFactory,
        AppProperties appProperties)
    {
        _telemetrySource = telemetrySource;
        _meterFactory = meterFactory;
        _meterName = appProperties.Name;
    }

    public void TrackEvent(ITelemetryEvent telemetryEvent, string userId, string correlationId)
    {
        var attributes = new Dictionary<string, object?>
        {
            [LoggingConstants.UserId] = userId,
            [LoggingConstants.CorrelationId] = correlationId
        };

        _activity = Activity.Current ??= _telemetrySource.Source.StartActivity();
        _activity?.AddEvent(new ActivityEvent(telemetryEvent.Name, tags: new ActivityTagsCollection(attributes)));
    }
    
    public void TrackMetric(ITelemetryMetric metric, double value)
    {
        var meter = _meterFactory.Create(_meterName);
        var counter = meter.CreateCounter<double>(metric.Name);
        counter.Add(value);
    }

    public void TrackMetric(ITelemetryMetric metric, double value, string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            TrackMetric(metric, value);
            return;
        }

        var meter = _meterFactory.Create(_meterName);
        var counter = meter.CreateCounter<double>(metric.Name);
        counter.Add(value, new KeyValuePair<string, object?>(LoggingConstants.UserId, userId));
    }

    public void TrackMetricWithDimension(
        ITelemetryMetric metric,
        double value,
        string dimensionName,
        string dimensionValue)
    {
        if (string.IsNullOrEmpty(dimensionName) || string.IsNullOrEmpty(dimensionValue))
        {
            TrackMetric(metric, value);
            return;
        }

        var meter = _meterFactory.Create(_meterName);
        var counter = meter.CreateCounter<double>(metric.Name);
        counter.Add(value, new KeyValuePair<string, object?>(dimensionName, dimensionValue));
    }

    public void TrackMetricWithDimension(
        ITelemetryMetric metric,
        double value,
        string dimensionName,
        string dimensionValue,
        string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            TrackMetricWithDimension(metric, value, dimensionName, dimensionValue);
            return;
        }

        if (string.IsNullOrEmpty(dimensionName) || string.IsNullOrEmpty(dimensionValue))
        {
            TrackMetric(metric, value, userId);
            return;
        }

        var meter = _meterFactory.Create(_meterName);
        var counter = meter.CreateCounter<double>(metric.Name);
        counter.Add(
            value,
            new KeyValuePair<string, object?>(dimensionName, dimensionValue),
            new KeyValuePair<string, object?>(LoggingConstants.UserId, userId));
    }

    public void Dispose()
    {
        _activity?.Dispose();
        GC.SuppressFinalize(this);
    }
}