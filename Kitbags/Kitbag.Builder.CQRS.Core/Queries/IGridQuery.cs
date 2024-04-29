using Kitbag.Builder.CQRS.Core.Queries.DTO;

namespace Kitbag.Builder.CQRS.Core.Queries;

public interface IGridQuery<T> : IFilteredQuery<PagedResult<T>>, ISortedQuery<PagedResult<T>>, IPagedQuery<T>, IGridQuery
{
}

public interface IGridQuery : IFilteredQuery, ISortedQuery, IPagedQuery
{
}