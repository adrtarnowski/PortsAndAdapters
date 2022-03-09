using Kitbag.Builder.CQRS.Core.Queries.DTO;

namespace Kitbag.Builder.CQRS.Core.Queries
{
    public interface IFilteredQuery<T> : IFilteredQuery, IQuery<T>
    {
    }

    public interface IFilteredQuery : IQuery
    {
        FilteringConfiguration? FilteringConfiguration { get; }
    }
}