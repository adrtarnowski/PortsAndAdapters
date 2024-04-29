using Kitbag.Builder.ServiceHealthCheck.Types;

namespace Kitbag.Builder.ServiceHealthCheck.Common;

public interface IKitbagHealthChecksBuilder
{
    IKitbagHealthChecksBuilder WithServiceHealthCheck(IServiceHealthCheck serviceHealthCheck);
}