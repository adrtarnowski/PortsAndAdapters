using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Kitbag.Builder.CQRS.Core.Queries;
using Newtonsoft.Json.Linq;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Providers;

public class DynamicParametersExtractor : IDynamicParametersExtractor
{
    public DynamicParameters ConfigureParameters(IGridQuery query)
    {
        var currentPage = query.PagingConfiguration?.Page.GetValueOrDefault() ?? 1;
        var pageSize = query.PagingConfiguration?.ItemsPerPage.GetValueOrDefault() ?? 100;
        var sortField = query.SortingConfiguration?.FirstOrDefault()?.Field;
        var sortOrder = query.SortingConfiguration?.FirstOrDefault()?.Order;
        var filters = query.FilteringConfiguration?.Filters.ToDictionary(f => f.Field, f => f.Value);

        ConvertArraysToStrings(filters!);

        var parameters = new DynamicParameters(filters);
        parameters.Add("offset", (currentPage - 1) * pageSize);
        parameters.Add("size", pageSize);
        parameters.Add("sortField", sortField);
        parameters.Add("sortOrder", sortOrder.ToString());
        return parameters;
    }

    private static void ConvertArraysToStrings(Dictionary<string, object> filters)
    {
        var arrayFilters = filters.Keys
            .Where(key => IsArray(filters, key))
            .ToList();
            
        foreach (var key in arrayFilters)
        {
            var array = (filters[key] as JArray)?.ToObject<string[]>();
                
            var enumerable = (filters[key] as IEnumerable<object>);
            array ??= enumerable?
                .Select(o => o.ToString() ?? string.Empty)
                .ToArray();
            array ??= new string[0];
                
            var stringValues = string.Join(";", array);
            if (!string.IsNullOrEmpty(stringValues))
            {
                filters[key] = stringValues;
            }
            else
            {
                filters.Remove(key);
            }
        }
    }

    private static bool IsArray(Dictionary<string, object> filters, string key)
    {
        return filters[key] is JArray || 
               filters[key] is IList || 
               filters[key].GetType().IsArray;
    }
}