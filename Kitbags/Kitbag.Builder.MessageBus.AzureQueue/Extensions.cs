using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.MessageBus.AzureQueue.Client;
using Kitbag.Builder.MessageBus.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.MessageBus.AzureQueue
{
    public static class Extensions
    {
        public static IKitbagBuilder AddAzureQueue(this IKitbagBuilder builder, string sectionName = "MessageBus")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            var busProperties = builder.GetSettings<BusProperties>(sectionName);
            builder.Services.AddSingleton(busProperties);
            builder.Services.AddSingleton<IAzureQueueClient, AzureQueueClient>();

            return builder;
        }
    }
}