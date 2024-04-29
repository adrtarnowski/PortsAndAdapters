using System.Collections.Generic;
using System.Linq;
using Kitbag.Builder.CQRS.Core.Queries.DTO;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Providers;

public static class FilterParametersProvider
{
    public static Dictionary<string, object>? GetArguments<TResult>(this FilteringConfiguration.Filter filter)
    {
        if (filter.Operator == FilteringConfiguration.FilterComparisonOperator.IsNotNull
            || filter.Operator == FilteringConfiguration.FilterComparisonOperator.IsNull)
        {
            return new Dictionary<string, object>();
        }

        if (filter.Operator == FilteringConfiguration.FilterComparisonOperator.In)
        {
            var fieldName = filter.GetFieldName<TResult>()!;
            var filterValues = GetFilterValuesList<TResult>(filter);
            if (filterValues == null)
                return null;

            var paramNames = GetInOperatorParamNames(fieldName, filterValues);

            return paramNames.Zip(filterValues, (n, v) => new { n, v })
                .ToDictionary(x => x.n, x => x.v);
        }

        if (filter.Operator == FilteringConfiguration.FilterComparisonOperator.Between)
        {
            var fieldName = filter.GetFieldName<TResult>()!;
            var filterValues = GetFilterValuesList<TResult>(filter);
            if (filterValues == null || filterValues.Count != 2)
                return null;

            var paramNames = GetBetweenOperatorParameterNames<TResult>(filter, fieldName);
            return paramNames.Zip(filterValues, (n, v) => new { n, v })
                .ToDictionary(x => x.n, x => x.v);
        }

        var paramName = filter.GetParameterName<TResult>();
        var value = GetFilterValue<TResult>(filter);

        var args = new Dictionary<string, object> { { paramName, value } };
        return args;
    }

    public static string? GetParameterName<TResult>(this FilteringConfiguration.Filter filter)
    {
        var fieldName = filter.GetFieldName<TResult>()!;

        switch (filter.Operator)
        {
            case FilteringConfiguration.FilterComparisonOperator.IsNotNull:
            case FilteringConfiguration.FilterComparisonOperator.IsNull:
                return "";
            case FilteringConfiguration.FilterComparisonOperator.In:
                return GetInOperatorParameterName<TResult>(filter, fieldName);
            case FilteringConfiguration.FilterComparisonOperator.Between:
                return GetBetweenOperatorParameterExpression<TResult>(filter, fieldName);
            default:
                return $"@Filtering_{fieldName}";
        }
    }

    private static string GetInOperatorParameterName<TResult>(FilteringConfiguration.Filter filter, string fieldName)
    {
        var filterValues = GetFilterValuesList<TResult>(filter);

        var paramNames = GetInOperatorParamNames(fieldName, filterValues);
        var inClause = string.Join(", ", paramNames);

        return $"({inClause})";
    }

    private static string? GetBetweenOperatorParameterExpression<TResult>(FilteringConfiguration.Filter filter, string fieldName)
    {
        var names = GetBetweenOperatorParameterNames<TResult>(filter, fieldName);
        return names == null
            ? null 
            : $"{names[0]} AND {names[1]}";
    }

    private static string[]? GetBetweenOperatorParameterNames<TResult>(FilteringConfiguration.Filter filter, string fieldName)
    {
        var filterValues = GetFilterValuesList<TResult>(filter);
        if (filterValues?.Count != 2)
            return null;
        return new[]
        {
            $"@Filtering_{fieldName}_Left",
            $"@Filtering_{fieldName}_Right"
        };
    }

    private static string[] GetInOperatorParamNames(string fieldName, List<object> filterValues)
    {
        return filterValues.Select(
            (s, i) => $"@Filtering_{fieldName}{i}"
        ).ToArray();
    }

    private static List<object>? GetFilterValuesList<TResult>(FilteringConfiguration.Filter filter)
    {
        if (!(filter.Value is IEnumerable<object> values))
            return null;

        var enumerable = values.Select(filter.ConvertToFieldType<TResult>).OfType<object>();
        return enumerable.ToList();
    }

    private static object? GetFilterValue<TResult>(FilteringConfiguration.Filter filter)
    {
        var value = filter.ConvertToFieldType<TResult>(filter.Value);
        if (filter.Operator == FilteringConfiguration.FilterComparisonOperator.Contains)
        {
            return $"%{value}%";
        }

        return value;
    }
}