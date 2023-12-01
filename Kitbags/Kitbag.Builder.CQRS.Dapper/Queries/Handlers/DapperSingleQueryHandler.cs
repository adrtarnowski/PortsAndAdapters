using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Kitbag.Builder.CQRS.Core.Queries;
using Kitbag.Builder.CQRS.Dapper.Split;
using Kitbag.Builder.CQRS.Dapper.Sql;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Handlers
{
    public abstract class DapperSingleQueryHandler<TQuery, TResult>
        : DapperQueryHandlerBase, IQueryHandler<TQuery, TResult>
        where TQuery : class, IDapperQuery<TResult>
        where TResult : class, new()
    {
        protected DapperSingleQueryHandler(ISqlConnectionFactory connectionFactory) : base(connectionFactory) { }

        public virtual async Task<TResult> HandleAsync(TQuery query)
        {
            ConfigureQuery(query);
            using var connection = _connectionFactory.CreateDbConnection();

            var template = query.SqlBuilder.AddTemplate(CreateSqlTemplate());
            return await connection.QuerySingleOrDefaultAsync<TResult>(template.RawSql, template.Parameters);
        }

        public virtual async Task<TResult> HandleQuerySplitAsync<TId>(TQuery query, SingleQuerySplitBuilder<TResult, TId> builder)
        {
            ConfigureQuery(query);
            using var connection = _connectionFactory.CreateDbConnection();

            var template = query.SqlBuilder.AddTemplate(CreateSqlTemplate());

            var func = builder.CreateSplitFunc();
            var types = builder.CreateTypesArray();
            var splitOn = builder.CreateSplitOnString();

            var results = await
                connection.QueryAsync(template.RawSql, types, func, template.Parameters, splitOn: splitOn);

            return results.FirstOrDefault()!;
        }
    }
}