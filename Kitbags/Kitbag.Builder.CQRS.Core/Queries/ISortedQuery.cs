using System.Collections.Generic;
using Kitbag.Builder.CQRS.Core.Queries.DTO;

namespace Kitbag.Builder.CQRS.Core.Queries
{
    public interface ISortedQuery<T> : ISortedQuery, IQuery<T>
    {
    }

    public interface ISortedQuery : IQuery
    {
        IEnumerable<SortingConfiguration>? SortingConfiguration { get; }
    }
}