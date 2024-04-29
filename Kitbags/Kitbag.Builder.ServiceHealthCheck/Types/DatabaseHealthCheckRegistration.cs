using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Persistence.Core.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck.Types;

public class DatabaseServiceHealthCheck : IServiceHealthCheck
{
    private readonly string _serviceName;

    public DatabaseServiceHealthCheck(string serviceName)
    {
        _serviceName = serviceName;
    }

    public void Register(IKitbagBuilder kitbagBuilder, IHealthChecksBuilder healthChecksBuilder)
    {
        if (kitbagBuilder is null) throw new ArgumentNullException($"{nameof(IKitbagBuilder)}");

        var databaseProperties = kitbagBuilder.GetSettings<PersistenceProperties>(_serviceName);

        if (databaseProperties?.ConnectionString is null)
            throw new ArgumentException(
                $"{typeof(PersistenceProperties)} could not be loaded from configuration. Please check, if section names are matching");

        healthChecksBuilder.AddSqlServer(
            databaseProperties.ConnectionString,
            name: _serviceName,
            tags: new[] { "Azure", "Database" }
        );
    }
}