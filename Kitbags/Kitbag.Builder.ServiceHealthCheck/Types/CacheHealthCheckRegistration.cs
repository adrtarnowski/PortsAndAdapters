using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Persistence.Core.Common;
using Kitbag.Builder.Redis.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck.Types;

public class RedisCacheHealthCheck : IServiceHealthCheck
{
    private readonly string _serviceName;

    public RedisCacheHealthCheck(string serviceName)
    {
        _serviceName = serviceName;
    }

    public void Register(IKitbagBuilder kitbagBuilder, IHealthChecksBuilder healthChecksBuilder)
    {
        if (kitbagBuilder is null) throw new ArgumentNullException($"{nameof(IKitbagBuilder)}");

        var redisProperties = kitbagBuilder.GetSettings<RedisProperties>(_serviceName);

        if (redisProperties?.Host is null || redisProperties?.Port is null || redisProperties?.Password is null)
            throw new ArgumentException($"{typeof(PersistenceProperties)} could not be loaded from configuration. Please check, if section names are matching");

        healthChecksBuilder.AddRedis(
            $"{redisProperties.Host}:{redisProperties.Port},password={redisProperties?.Password},ssl={redisProperties?.Ssl},abortConnect=False",
            name: _serviceName,
            tags: new[] { "Azure", "Redis" }
        );
    }
}