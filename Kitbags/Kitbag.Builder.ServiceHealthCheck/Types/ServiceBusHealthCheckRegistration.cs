using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.MessageBus.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck.Types;

public class ServiceBusHealthCheck : IServiceHealthCheck
{
    private readonly string _serviceName;

    public ServiceBusHealthCheck(string serviceName)
    {
        _serviceName = serviceName;
    }

    public void Register(IKitbagBuilder kitbagBuilder, IHealthChecksBuilder healthChecksBuilder)
    {
        if (kitbagBuilder is null) throw new ArgumentNullException($"{nameof(IKitbagBuilder)}");

        var busProperties = kitbagBuilder.GetSettings<BusProperties>(_serviceName);

        if (busProperties?.ConnectionString is null)
            throw new ArgumentException($"{typeof(BusProperties)} could not be loaded from configuration. Please check, if section names are matching");

        if (busProperties?.EventTopicName != null)
        {
            healthChecksBuilder.AddAzureServiceBusTopic(
                busProperties.ConnectionString,
                busProperties.EventTopicName,
                name: $"Service Bus Topic {busProperties.EventTopicName}",
                tags: new[] { "Azure", "AzureServiceBus" });
        }
    }
}