using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.Redis.Common;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Kitbag.Builder.Redis
{
    public static class Extensions
    {
        public static IKitbagBuilder AddRedisCacheIntegration(this IKitbagBuilder builder, string sectionName = "Redis")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            
            var redisProperties = builder.GetSettings<RedisProperties>(sectionName);
            var appProperties = builder.GetSettings<AppProperties>(sectionName);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = appProperties.InstanceId.ToString();
                options.ConfigurationOptions = new ConfigurationOptions()
                {
                    EndPoints = { $"{redisProperties.Host}:{redisProperties.Port}" },
                    Ssl = redisProperties.Ssl,
                    Password = redisProperties.Password
                };
            });

            return builder;
        }
    }
}