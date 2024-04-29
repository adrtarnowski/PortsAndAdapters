using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Kitbag.Builder.CQRS.Core.Queries;
using Kitbag.Builder.CQRS.Dapper.Sql;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Handlers;

public abstract class DapperQueryWithProcedureHandler<TQuery, TResult>
    : DapperQueryHandlerBase, IQueryHandler<TQuery, IEnumerable<TResult>>
    where TQuery : class, IDapperQuery<IEnumerable<TResult>>
    where TResult : class, new()
{
    protected DapperQueryWithProcedureHandler(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public virtual async Task<IEnumerable<TResult>> HandleAsync(TQuery query)
    {
        using var connection = _connectionFactory.CreateDbConnection();
        var parameters = GetParameters(query);

        return (await connection.QueryAsync<TResult>(
            TableOrViewName,
            parameters,
            commandType: CommandType.StoredProcedure)).ToList();
    }

    protected virtual object? GetParameters(TQuery query)
    {
        return null;
    }
}