﻿using System;
using System.Threading.Tasks;
using Azure.Identity;
using Kitbag.Builder.Core;
using Kitbag.Builder.HttpClient;
using Kitbag.Builder.Logging.AppInsights;
using Kitbag.Builder.ServiceHealthCheck;
using Kitbag.Builder.Swagger;
using Kitbag.Builder.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using TLJ.PortsAndAdapters.Infrastructure;
using Microsoft.Extensions.Configuration;
using static System.String;

namespace TLJ.PortsAndAdapters.Api
{
    class Program
    {
        public static Task Main(string[] args)
            => CreateWebHostBuilder(args).Build().RunAsync();
        
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var settings = config.Build();
                    var connectionString = settings["AppConfiguration:ConnectionString"];
                    var appConfigEndpoint = settings["AppConfiguration:Endpoint"];
                    if (!IsNullOrEmpty(connectionString))
                    {
                        config.AddAzureAppConfiguration(options => options.Connect(connectionString));
                    }
                    else if(!IsNullOrEmpty(appConfigEndpoint))
                    {
                        config.AddAzureAppConfiguration(options => options.Connect(new Uri(appConfigEndpoint), new ManagedIdentityCredential()));
                    }
                })
                .ConfigureServices((webHostBuilderContext, services) => services
                    .AddKitbag(webHostBuilderContext.Configuration)
                    .AddWebApi()
                    .AddApiContext()
                    .AddSwagger()
                    .AddAppInsights()
                    .AddHttpClient()
                    .AddServiceHealthChecks()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseKitbag()
                    .UseErrorHandler()
                    .UseSwaggerDoc()
                    .UseControllers()
                    .UseInfrastructure()
                );
        }
    }
}