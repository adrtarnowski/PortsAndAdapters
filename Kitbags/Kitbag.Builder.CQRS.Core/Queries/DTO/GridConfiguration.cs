using System.Collections.Generic;

namespace Kitbag.Builder.CQRS.Core.Queries.DTO
{
    public class GridConfiguration
    {
        public FilteringConfiguration? Filtering { get; set; }
        public IEnumerable<SortingConfiguration>? Sorting { get; set; }
        public PagingConfiguration? Paging { get; set; }
        public object? CustomFilters { get; set; }
    }
}