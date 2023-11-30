﻿using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.ServiceHealthCheck.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck
{
    public static class Extensions
    {
        public static IKitbagBuilder AddServiceHealthChecks(this IKitbagBuilder builder,
            string sectionName = "HealthChecks")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;

            builder.Services
                .AddHealthChecks()
                .RegisterDatabaseHealthCheck(builder, "database");
                //.RegisterRedisHealthCheck(builder,"database");
            return builder;
        }
    }
}