using System;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.Core.Types
{
    public interface IKitbagBuilder
    {
        IServiceCollection Services { get; }

        bool TryRegisterKitBag(string kitBagName);
        
        IServiceProvider Build();
        
        void AddInitializer<TInitializer>() where TInitializer : IInitializer;
        
        TProperties GetSettings<TProperties>(string appSettingSectionName) where TProperties : new();

        void GetSettings<TProperties>(string appSettingSectionName, TProperties properties) where TProperties : new();
    }
}