using System;
using System.Collections.Generic;
using System.Linq;
using Kitbag.Builder.Core.Initializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.Core.Builders
{
    public class KitbagBuilder : IKitbagBuilder
    {
        public IServiceCollection Services { get; }
        private readonly List<Action<IServiceProvider>> _buildActions;
        private readonly IConfiguration? _configuration;
        private readonly List<string> _registeredKitbags;

        public KitbagBuilder(IServiceCollection services, IConfiguration? configuration = null)
        {
            Services = services;
            Services.AddSingleton<IStartupInitializer>(new StartupInitializer());
            _registeredKitbags = new List<string>();
            _buildActions = new List<Action<IServiceProvider>>();

            if (configuration == null)
            {
                using var serviceProvider = Services.BuildServiceProvider();
                _configuration = serviceProvider.GetService<IConfiguration>();
            }
            else
            {
                _configuration = configuration;
            }
        }
        
        public bool TryRegisterKitBag(string kitBagName)
        {
            var isAlreadyRegistered = _registeredKitbags.Any(r => r == kitBagName);
            if (isAlreadyRegistered)
                return false;
            _registeredKitbags.Add(kitBagName);
            return true;
        }

        public IServiceProvider Build()
        {
            var serviceProvider = Services.BuildServiceProvider();
            _buildActions.ForEach(a => a(serviceProvider));
            return serviceProvider;
        }

        public void AddInitializer<TInitializer>() where TInitializer : IInitializer
        {
            BuildAction(sp =>
            {
                var initializer = sp.GetService<TInitializer>();
                var startupInitializer = sp.GetService<IStartupInitializer>();
                if (startupInitializer != null) startupInitializer.AddInitializer(initializer);
            });
        }

        public TProperties GetSettings<TProperties>(string appSettingSectionName) where TProperties : new()
        {
            return _configuration.GetSettings<TProperties>(appSettingSectionName);
        }

        public void GetSettings<TProperties>(string appSettingSectionName, TProperties properties)
            where TProperties : new()
        {
            if (_configuration != null) _configuration.GetSection(appSettingSectionName).Bind(properties);
        }
        
        private void BuildAction(Action<IServiceProvider> execute)
        {
            _buildActions.Add(execute);
        }
    }
}