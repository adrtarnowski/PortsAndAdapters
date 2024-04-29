using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Queries;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Handlers;

public class PagedQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _baseQueryHandler;

    public PagedQueryHandler(IQueryHandler<TQuery, TResult> baseQueryHandler)
    {
        _baseQueryHandler = baseQueryHandler;
    }

    public async Task<TResult> HandleAsync(TQuery query)
    {
        if ((query is IDapperQuery dapperQuery) && (dapperQuery is IPagedQuery pagedQuery) && pagedQuery.PagingConfiguration != null)
        {
            AddPaginationToQueryBuilder(dapperQuery, pagedQuery);
        }

        return await _baseQueryHandler.HandleAsync(query);
    }

    private static void AddPaginationToQueryBuilder(IDapperQuery dapperQuery, IPagedQuery pagedQuery)
    {
        var sqlBuilder = dapperQuery.SqlBuilder;
        var config = pagedQuery.PagingConfiguration!;

        var offset = config.ItemsPerPage * (config.Page - 1);

        sqlBuilder.Offset("@Paging_Offset", new { Paging_Offset = offset });
        sqlBuilder.FetchNext("@Paging_FetchNext", new { Paging_FetchNext = config.ItemsPerPage});
    }
}