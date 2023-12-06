using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Persistence.Core.Common;
using Kitbag.Builder.Redis.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck.Types
{
    public static class CacheHealthCheckRegistration
    {
        public static IHealthChecksBuilder RegisterRedisHealthCheck(
            this IHealthChecksBuilder healthChecksBuilder,
            IKitbagBuilder kitbagBuilder,
            string serviceName)
        {
            if (kitbagBuilder is null) throw new ArgumentNullException($"{nameof(IKitbagBuilder)}");

            var redisProperties = kitbagBuilder.GetSettings<RedisProperties>(serviceName);

            if (redisProperties?.Host is null || redisProperties?.Port is null || redisProperties?.Password is null)
                throw new ArgumentException($"{typeof(PersistenceProperties)} could not be loaded from configuration. Please check, if section names are matching");

            healthChecksBuilder.AddRedis(
                $"{redisProperties.Host}:{redisProperties.Port},password={redisProperties?.Password},ssl={redisProperties?.Ssl},abortConnect=False",
                name: serviceName,
                tags: new[] { "Azure", "Redis" }
            );
            return healthChecksBuilder;
        }
    }
}