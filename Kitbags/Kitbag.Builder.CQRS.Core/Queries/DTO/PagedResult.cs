using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Kitbag.Builder.CQRS.Core.Queries.DTO;

public class PagedResult<T> : PagedResultBase
{
    public IEnumerable<T> Items { get; }
    public bool IsEmpty => Items == null || !Items.Any();

    protected PagedResult()
    {
        Items = Enumerable.Empty<T>();
    }

    [JsonConstructor]
    protected PagedResult(
        IEnumerable<T> items,
        int currentPage, 
        int resultsPerPage,
        int totalPages, 
        long totalResults) : base(currentPage, resultsPerPage, totalPages, totalResults)
    {
        Items = items;
    }

    public static PagedResult<T> Create(
        IEnumerable<T> items,
        int currentPage, 
        int resultsPerPage,
        int totalPages, 
        long totalResults)
    {
        return new PagedResult<T>(items, currentPage, resultsPerPage, totalPages, totalResults);
    }
}