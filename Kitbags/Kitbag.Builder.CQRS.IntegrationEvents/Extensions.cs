using System;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.CQRS.IntegrationEvents.Common;
using Kitbag.Builder.CQRS.IntegrationEvents.Dispatchers;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.CQRS.IntegrationEvents
{
    public static class Extensions
    {
        public static IKitbagBuilder AddCQRSIntegrationEvents(this IKitbagBuilder builder, string sectionName = "CQRS.IntegrationEvents")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            
            builder.AddIntegrationEventHandlers();
            builder.AddInMemoryIntegrationEventDispatcher();
            return builder;
        }
        
        private static IKitbagBuilder AddIntegrationEventHandlers(this IKitbagBuilder builder)
        {
            builder.Services.Scan(s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventHandler<>)))
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .WithTransientLifetime());
            return builder;
        }
        
        private static IKitbagBuilder AddInMemoryIntegrationEventDispatcher(this IKitbagBuilder builder)
        {
            builder.Services.AddSingleton<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
            return builder;
        }
    }
}