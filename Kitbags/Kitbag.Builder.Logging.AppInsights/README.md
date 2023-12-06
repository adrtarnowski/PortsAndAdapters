## :chart_with_upwards_trend: Logging 

Kitbag provides integration with AppInsights

## Getting Started

To use `Kitbag.Builder.Logging.AppInsights`, you need to add the following line in `Extension` class in the `TLJ.PortsAndAdapters.Infrastructure` project:

```
public static IKitbagBuilder AddInfrastructure(this IKitbagBuilder builder)
{
    ...
    builder.AddAppInsights();
    ...
``` 

Provide necessary configuration in _appsettings.json_ file in _**PortsAndAdapters.Api**_ project
```
    "Logging": {
        "ApplicationInsights": {`
            "InstrumentationKey": "<Provide key>"
        },
        "LogLevel": {
        "Default": "Debug"
        }
    },
```

## How to use?

- Declare dependency in _Extensions.cs_ file in _**PortsAndAdapters.Infrastructure**_ project (_AddInfrastructure_ method)
```
    builder.AddAppInsights();
```

- After necessary declaration you can use ILogger<T> in your classes and collect your log data in AppInsights instance
```
    public class SomeClass
    {
        private readonly ILogger<SomeClass> _logger;

        public SomeClass(ILogger<SomeClass> logger)
        {
            _logger = logger;
        }
    }
```

- Optionally you can use the following declaration to decorate all your incoming requests
```
    builder.Services.Decorate(typeof(ICommandHandler<>), typeof(AppInsightLoggingCommandHandlerDecorator<>));
```
- There is a possibility to log telemetry data via AppInsights client
```
            private readonly IAppInsightsClient _appInsights;
               
             ...
             
            _appInsights.TrackMetricWithDimension(
                CommandHandlerMetric.CommandRequested,
                1,
                CommandHandlerMetric.CommandNameDimension,
                operation.GetType().Name,
                userId);
```
