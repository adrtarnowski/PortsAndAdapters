using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Kitbag.Builder.CQRS.Core.Queries;
using Kitbag.Builder.CQRS.Core.Queries.DTO;
using Kitbag.Builder.CQRS.Dapper.Queries.Providers;
using Kitbag.Builder.CQRS.Dapper.Sql;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Handlers;

public abstract class DapperPagedQueryWithProcedureHandler<TQuery, TResult> :
    DapperQueryHandlerBase, IQueryHandler<TQuery, PagedResult<TResult>>
    where TQuery : DapperGridQuery<TResult>
    where TResult : IResultWithTotalRows
{
    private readonly IDynamicParametersExtractor _dynamicParametersExtractor;

    protected DapperPagedQueryWithProcedureHandler(ISqlConnectionFactory connectionFactory, IDynamicParametersExtractor dynamicParametersExtractor) : base(connectionFactory)
    {
        _dynamicParametersExtractor = dynamicParametersExtractor;
    }

    public virtual async Task<PagedResult<TResult>> HandleAsync(TQuery query)
    {
        var items = await GetFilteredResultsAsync(query);
        var itemsPerPage = query.PagingConfiguration?.ItemsPerPage ?? throw new ArgumentNullException(nameof(query.PagingConfiguration.ItemsPerPage));
        var totalRows = items.FirstOrDefault()?.TotalRows ?? 0;
        var totalPages = (int)Math.Ceiling((double)totalRows / itemsPerPage);
        var page = query.PagingConfiguration?.Page ?? throw new ArgumentNullException(nameof(query.PagingConfiguration.Page));
        return PagedResult<TResult>.Create(items, page, itemsPerPage, totalPages, totalRows);
    }

    protected async Task<IEnumerable<TResult>> GetFilteredResultsAsync(DapperGridQuery<TResult> query)
    {
        var parameters = _dynamicParametersExtractor.ConfigureParameters(query);
        using var connection = _connectionFactory.CreateDbConnection();

        return (await connection.QueryAsync<TResult>(
            TableOrViewName,
            parameters,
            commandType: CommandType.StoredProcedure)).ToList();
    }
}