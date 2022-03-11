using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.MessageBus.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Kitbag.Builder.MessageBus.ServiceBus.Clients;
using Kitbag.Builder.MessageBus.ServiceBus.Common;
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
            builder.Services.AddSingleton<IBusClient, BusClient>();
            builder.Services.AddSingleton<IBusPublisher, ServiceBusPublisher>();
            builder.Services.AddSingleton<IBusSubscriptionsManager, BusSubscriptionManager>();
            builder.Services.AddSingleton<IEventBusSubscriber, ServiceBusEventSubscriber>();

            return builder;
        }
    }
}