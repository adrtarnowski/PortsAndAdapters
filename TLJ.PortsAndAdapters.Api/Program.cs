using System;
using System.Threading.Tasks;
using Azure.Identity;
using Kitbag.Builder.Core;
using Kitbag.Builder.HttpClient;
using Kitbag.Builder.Logging.OpenTelemetry;
using Kitbag.Builder.ServiceHealthCheck;
using Kitbag.Builder.ServiceHealthCheck.Types;
using Kitbag.Builder.Swagger;
using Kitbag.Builder.WebApi;
using Microsoft.AspNetCore.Builder;
using TLJ.PortsAndAdapters.Infrastructure;
using Microsoft.Extensions.Configuration;
using static System.String;

namespace TLJ.PortsAndAdapters.Api;

class Program
{
    public static Task Main(string[] args)
        => CreateWebApplication(args).RunAsync();

    public static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Inititalize configuration settings and its source
        var connectionString = builder.Configuration["AppConfiguration:ConnectionString"];
        var appConfigEndpoint = builder.Configuration["AppConfiguration:Endpoint"];
        if (!IsNullOrEmpty(connectionString))
        {
            builder.Configuration.AddAzureAppConfiguration(options => options.Connect(connectionString));
        }
        else if (!IsNullOrEmpty(appConfigEndpoint))
        {
            builder.Configuration.AddAzureAppConfiguration(options =>
                options.Connect(new Uri(appConfigEndpoint), new ManagedIdentityCredential()));
        }

        // Add Kitbag and its services
        builder.Services
            .AddKitbag(builder.Configuration)
            .AddWebApi()
            .AddApiContext()
            .AddSwagger()
            .AddOpenTelemetry()
            .AddHttpClient()
            .AddServiceHealthChecks(healthChecksBuilder => 
                healthChecksBuilder.WithServiceHealthCheck(new DatabaseServiceHealthCheck("Database")))
            .AddInfrastructure()
            .Build();

        var app = builder.Build();

        app.UseKitbag()
            .UseErrorHandler()
            .UseSwaggerDoc()
            .UseControllers()
            .UseInfrastructure();

        app.MapControllers();
        return app;
    }
}