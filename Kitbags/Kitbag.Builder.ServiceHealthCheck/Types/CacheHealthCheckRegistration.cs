using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Persistence.Core.Common;
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

            var databaseProperties = kitbagBuilder.GetSettings<PersistenceProperties>(serviceName);

            if (databaseProperties?.ConnectionString is null)
                throw new ArgumentException($"{typeof(PersistenceProperties)} could not be loaded from configuration. Please check, if section names are matching");

            healthChecksBuilder.AddRedis(
                databaseProperties.ConnectionString,
                name: serviceName,
                tags: new[] { "Azure", "Redis" }
            );
            return healthChecksBuilder;
        }
    }
}