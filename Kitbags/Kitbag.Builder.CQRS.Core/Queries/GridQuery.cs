using System.Collections.Generic;
using Kitbag.Builder.CQRS.Core.Queries.DTO;

namespace Kitbag.Builder.CQRS.Core.Queries;

public abstract class GridQuery<T> : IGridQuery<T>
{
    protected GridQuery(GridConfiguration gridConfiguration)
    {
        FilteringConfiguration = gridConfiguration.Filtering;
        SortingConfiguration = gridConfiguration.Sorting;
        PagingConfiguration = gridConfiguration.Paging;
        CustomFilters = gridConfiguration.CustomFilters;
    }

    public FilteringConfiguration? FilteringConfiguration { get; }
    public IEnumerable<SortingConfiguration>? SortingConfiguration { get; }
    public PagingConfiguration? PagingConfiguration { get; }
    public object? CustomFilters { get; }
}