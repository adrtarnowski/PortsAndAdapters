using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.MessageBus.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck.Types
{
    public class MessageBrokerHealthCheckRegistration
    {
        public static IHealthChecksBuilder RegisterHealthCheck(
            IHealthChecksBuilder healthChecksBuilder, 
            IKitbagBuilder kitbagBuilder, 
            string sectionName)
        {
            if (kitbagBuilder is null) throw new ArgumentNullException($"{nameof(IKitbagBuilder)}");

            var busProperties = kitbagBuilder.GetSettings<BusProperties>(sectionName);

            if (busProperties?.ConnectionString is null)
                throw new ArgumentException($"{typeof(BusProperties)} could not be loaded from configuration. Please check, if section names are matching");

            if (busProperties?.EventTopicName != null)
            {
                healthChecksBuilder.AddAzureServiceBusTopic(
                    busProperties.ConnectionString,
                    busProperties.EventTopicName,
                    $"Service Bus Topic {busProperties.EventTopicName}",
                    tags: new[] { "Azure", "AzureServiceBus" });
            }

            return healthChecksBuilder;
        }
    }
}