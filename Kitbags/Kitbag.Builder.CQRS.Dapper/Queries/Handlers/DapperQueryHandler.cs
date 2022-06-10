using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Kitbag.Builder.CQRS.Core.Queries;
using Kitbag.Builder.CQRS.Dapper.Split;
using Kitbag.Builder.CQRS.Dapper.Sql;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Handlers
{
    public abstract class DapperQueryHandler<TQuery, TResult>
        : DapperQueryHandlerBase, IQueryHandler<TQuery, IEnumerable<TResult>>
        where TQuery : class, IDapperQuery<IEnumerable<TResult>>
        where TResult : class, new()
    {
        protected DapperQueryHandler(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public virtual async Task<IEnumerable<TResult>> HandleAsync(TQuery query)
        {
            ConfigureQuery(query);
            using var connection = _connectionFactory.CreateDbConnection();

            var template = query.SqlBuilder.AddTemplate(CreateSqlTemplate());
            return await connection.QueryAsync<TResult>(template.RawSql, template.Parameters);
        }

        public virtual async Task<IEnumerable<TResult>> HandleQuerySplitAsync<TId>(TQuery query, MultipleQuerySplitBuilder<TResult, TId> builder)
        {
            ConfigureQuery(query);
            using var connection = _connectionFactory.CreateDbConnection();

            var template = query.SqlBuilder.AddTemplate(CreateSqlTemplate());

            var func = builder.CreateSplitFunc();
            var types = builder.CreateTypesArray();
            var splitOn = builder.CreateSplitOnString();

            var results = await
                connection.QueryAsync(template.RawSql, types, func, template.Parameters, splitOn: splitOn);

            return results.Distinct();
        }
    }
}