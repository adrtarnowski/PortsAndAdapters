using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.ServiceHealthCheck.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck.Common;

public class KitbagHealthChecksBuilder : IKitbagHealthChecksBuilder
{
    private readonly List<IServiceHealthCheck> _serviceHealthChecks = new();
    private readonly IKitbagBuilder _kitbagBuilder;

    public KitbagHealthChecksBuilder(IKitbagBuilder kitbagBuilder)
    {
        _kitbagBuilder = kitbagBuilder;
    }

    public IKitbagHealthChecksBuilder WithServiceHealthCheck(IServiceHealthCheck serviceHealthCheck)
    {
        _serviceHealthChecks.Add(serviceHealthCheck);
        return this;
    }

    public IHealthChecksBuilder Build()
    {
        var healthChecksBuilder = _kitbagBuilder.Services.AddHealthChecks();
        foreach (var serviceHealthCheck in _serviceHealthChecks)
        {
            serviceHealthCheck.Register(_kitbagBuilder, _kitbagBuilder.Services.AddHealthChecks());
        }

        return healthChecksBuilder;
    }
}