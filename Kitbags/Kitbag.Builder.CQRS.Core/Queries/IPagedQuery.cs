using Kitbag.Builder.CQRS.Core.Queries.DTO;

namespace Kitbag.Builder.CQRS.Core.Queries
{
    public interface IPagedQuery<T> : IPagedQuery, IQuery<PagedResult<T>>
    {
    }

    public interface IPagedQuery : IQuery
    {
        PagingConfiguration? PagingConfiguration { get; }
    }
}