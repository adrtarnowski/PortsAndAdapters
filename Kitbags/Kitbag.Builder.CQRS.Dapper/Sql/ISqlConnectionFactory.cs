using System.Data;

namespace Kitbag.Builder.CQRS.Dapper.Sql;

public interface ISqlConnectionFactory
{
    IDbConnection CreateDbConnection();
}