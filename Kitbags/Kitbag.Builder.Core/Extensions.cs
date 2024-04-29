using System;
using System.Threading.Tasks;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.Core.Initializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.Core;

public static class Extensions
{
    /// <summary>
    /// Kitbags are a set of packages that contain reusable plugins.
    /// </summary>
    /// <param name="services">Service collection from Microsoft.Extensions</param>
    /// <param name="sectionName">Unique section name for registration</param>
    /// <returns></returns>
    public static IKitbagBuilder AddKitbag(this IServiceCollection services, IConfiguration? configuration = null,
        string sectionName = "App")
    {
        var builder = new KitbagBuilder(services, configuration);
        var properties = builder.GetSettings<AppProperties>(sectionName);
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton(properties);
        Console.WriteLine($"App: {properties.Name} | Version: {properties.Version} is up and running | InstanceId: {properties.InstanceId}");
        return builder;
    }
        
    public static IApplicationBuilder UseKitbag(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var initializer = scope.ServiceProvider.GetService<IStartupInitializer>();
        if (initializer is null)
            throw new InvalidOperationException("Startup initializer was not found");
        Task.Run(() => AsyncHelper.RunSync(() => initializer.InitializeAsync()));
        return builder;
    }
        
    public static TProperties GetSettings<TProperties>(this IConfiguration configuration, string appSettingSectionName) where TProperties : new()
    {
        var properties = new TProperties();
        configuration.GetSection(appSettingSectionName).Bind(properties);
        return properties;
    }
}