using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Queries;
using Kitbag.Builder.CQRS.Dapper.Queries.Providers;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Handlers
{
    public class SortedQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _baseQueryHandler;

        public SortedQueryHandler(IQueryHandler<TQuery, TResult> baseQueryHandler)
        {
            _baseQueryHandler = baseQueryHandler;
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            if ((query is IDapperQuery<TResult> dapperQuery) && (dapperQuery is ISortedQuery<TResult> sortedQuery) && sortedQuery.SortingConfiguration != null)
            {
                AddSortingToSqlBuilder(dapperQuery, sortedQuery);
            }

            return await _baseQueryHandler.HandleAsync(query);
        }

        private static void AddSortingToSqlBuilder(IDapperQuery<TResult> dapperQuery, ISortedQuery<TResult> sortedQuery)
        {
            var sqlBuilder = dapperQuery.SqlBuilder;

            foreach (var configuration in sortedQuery.SortingConfiguration!)
            {
                var sortDirection = configuration.GetSortDirection();
                var fieldName = configuration.GetFieldName<TResult>()!;

                sqlBuilder.OrderBy($"{fieldName} {sortDirection}");
            }
        }
    }
}