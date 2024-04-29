using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.ServiceHealthCheck.Common;
using Kitbag.Builder.ServiceHealthCheck.Types;

namespace Kitbag.Builder.ServiceHealthCheck;

public static class Extensions
{
    public static IKitbagBuilder AddServiceHealthChecks(
        this IKitbagBuilder kitbagBuilder,
        Action<IKitbagHealthChecksBuilder>? buildAction = null,
        string sectionName = "HealthChecks")
    {
        if (!kitbagBuilder.TryRegisterKitBag(sectionName))
            return kitbagBuilder;
        var kitbagHealthChecksBuilder = new KitbagHealthChecksBuilder(kitbagBuilder);
        buildAction?.Invoke(kitbagHealthChecksBuilder);
        kitbagHealthChecksBuilder.Build();
        return kitbagBuilder;
    }

    public static IKitbagBuilder AddServiceHealthChecks(
        this IKitbagBuilder kitbagBuilder,
        string sectionName = "HealthChecks")
    {
        return kitbagBuilder.AddServiceHealthChecks(
            builder => builder.WithServiceHealthCheck(new DatabaseServiceHealthCheck("Database")),
            sectionName);
    }
}