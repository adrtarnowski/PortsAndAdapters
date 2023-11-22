## Logging :chart_with_upwards_trend:

Kitbag provides integration with AppInsights

## How to use?
- Provide necessary configuration in _appsettings.json_ file in _**PortsAndAdapters.Api**_ project
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

- Declare dependency in _Extensions.cs_ file in _**PortsAndAdapters.Infrastructure**_ project (_AddInfrastructure_ method)
```
    builder.AddAppInsights();
```

- After declaration you can use ILogger<T> in your classes and collect your log data in AppInsights instance
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
