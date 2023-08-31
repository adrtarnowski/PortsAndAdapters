using Azure.Messaging.ServiceBus;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.MessageBus.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Kitbag.Builder.MessageBus.ServiceBus.Common;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.MessageBus.ServiceBus
{
    public static class Extensions
    {
        public static IKitbagBuilder AddServiceBus(this IKitbagBuilder builder, string sectionName = "MessageBus")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            var busProperties = builder.GetSettings<BusProperties>(sectionName);
            builder.Services.AddSingleton(busProperties);
            builder.Services.AddSingleton<IBusSubscriptionsManager, BusSubscriptionManager>();
            
            builder.Services.AddAzureClients(sbBuilder =>
            {
                sbBuilder.AddServiceBusClient(busProperties.ConnectionString);
                sbBuilder.AddServiceBusAdministrationClient(busProperties.ConnectionString);
            });

            return builder;
        }
        
        public static IKitbagBuilder AddServiceBusPublisher<T>(this IKitbagBuilder builder, string sectionName = "ServiceBusPublisher") where T : IIntegrationEvent
        {
            var eventType = MessageBus.Extensions.GetEventFor<T>();
            if (!builder.TryRegisterKitBag($"{eventType}_publisher"))
                return builder;
            var busProperties = builder.GetSettings<BusProperties>(sectionName);
            builder.Services.AddSingleton<IBusPublisher<T>, ServiceBusPublisher<T>>();
            builder.Services.AddAzureClients(sbBuilder =>
            {
                sbBuilder.AddClient<ServiceBusSender, ServiceBusClientOptions>((_, provider) =>
                    {
                        var busSettings = provider.GetRequiredService<BusProperties>();
                        return provider
                            .GetService<ServiceBusClient>()!
                            .CreateSender(busSettings.EventTopicName);
                    })
                    .WithName(eventType);
            });
            return builder;
        }
        
        public static IKitbagBuilder AddServiceBusSubscriber(this IKitbagBuilder builder, string sectionName = "ServiceBusSubscriber")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            builder.Services.AddSingleton<IEventSubscriber, ServiceBusEventSubscriber>();
            
            return builder;
        }
        
        public static IKitbagBuilder AddServiceBusWorker(this IKitbagBuilder builder, string sectionName = "ServiceBusWorker")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            builder.Services.AddHostedService<WorkerServiceBus>();
            return builder;
        }
    }
}