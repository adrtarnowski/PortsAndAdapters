using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Queries;
using Kitbag.Builder.CQRS.Core.Queries.DTO;
using Kitbag.Builder.CQRS.Dapper.Queries.Providers;
using Kitbag.Builder.CQRS.Dapper.Sql;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Handlers
{
    public class FilteredQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _baseQueryHandler;

        public FilteredQueryHandler(IQueryHandler<TQuery, TResult> baseQueryHandler)
        {
            _baseQueryHandler = baseQueryHandler;
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            if ((query is IDapperQuery<TResult> dapperQuery) && (dapperQuery is IFilteredQuery<TResult> filteredQuery) && filteredQuery.FilteringConfiguration != null)
            {
                AddFiltersToQueryBuilder(dapperQuery, filteredQuery);
            }

            return await _baseQueryHandler.HandleAsync(query);
        }

        private static void AddFiltersToQueryBuilder(IDapperQuery<TResult> dapperQuery, IFilteredQuery<TResult> filteredQuery)
        {
            var sqlBuilder = dapperQuery.SqlBuilder;
            foreach (var filter in filteredQuery.FilteringConfiguration!.Filters!)
            {
                var args = filter.GetArguments<TResult>();
                var filterSql = GetFilterSql(filter);
                AddSqlBuilderWhereClause(filteredQuery, sqlBuilder, filterSql, args);
            }
        }

        private static string GetFilterSql(FilteringConfiguration.Filter filter)
        {
            var sqlOperator = filter.GetSqlComparisonOperator();
            var fieldName = filter.GetFieldName<TResult>()!;
            var paramName = filter.GetParameterName<TResult>();

            if (filter.Operator == FilteringConfiguration.FilterComparisonOperator.DateEquals)
            {
                return $"CAST({fieldName} AS DATE) {sqlOperator} CAST({paramName} AS DATE)";
            }

            return $"{fieldName} {sqlOperator} {paramName}".Trim();
        }

        private static void AddSqlBuilderWhereClause(IFilteredQuery<TResult> filteredQuery, ISqlBuilder sqlBuilder,
            string filterSql, Dictionary<string, object> args)
        {
            switch (filteredQuery.FilteringConfiguration!.Operator)
            {
                case FilteringConfiguration.FilterLogicalOperator.And:
                    sqlBuilder.Where(filterSql, args);
                    break;
                case FilteringConfiguration.FilterLogicalOperator.Or:
                    sqlBuilder.OrWhere(filterSql, args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
