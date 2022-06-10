using System.Data;
using Kitbag.Builder.Persistence.Core.Common;
using Microsoft.Data.SqlClient;

namespace Kitbag.Builder.CQRS.Dapper.Sql
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(PersistenceProperties databaseProperties)
        {
            _connectionString = databaseProperties.ConnectionString!;
        }

        public IDbConnection CreateDbConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}