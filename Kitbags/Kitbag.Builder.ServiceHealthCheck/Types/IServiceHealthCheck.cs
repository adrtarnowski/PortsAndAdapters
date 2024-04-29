using Kitbag.Builder.Core.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck.Types;

public interface IServiceHealthCheck
{
    void Register(IKitbagBuilder kitbagBuilder, IHealthChecksBuilder healthChecksBuilder);
}