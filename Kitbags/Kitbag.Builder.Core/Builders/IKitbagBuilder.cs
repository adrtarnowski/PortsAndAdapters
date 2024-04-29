using System;
using Kitbag.Builder.Core.Initializer;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.Core.Builders;

public interface IKitbagBuilder
{
    IServiceCollection Services { get; }

    bool TryRegisterKitBag(string kitBagName);
        
    IServiceProvider Build();
        
    void AddInitializer<TInitializer>() where TInitializer : IInitializer;
        
    TProperties GetSettings<TProperties>(string appSettingSectionName) where TProperties : new();

    void GetSettings<TProperties>(string appSettingSectionName, TProperties properties) where TProperties : new();
}