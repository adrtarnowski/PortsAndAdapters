using System;
using System.Linq;
using System.Reflection;
using Kitbag.Builder.CQRS.Core.Queries.DTO;
using Newtonsoft.Json.Linq;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Providers;

internal static class QueryFieldProvider
{
    public static object? ConvertToFieldType<TResult>(this FilteringConfiguration.Filter filter, object? value)
    {
        try
        {
            var fieldType = GetFieldType<TResult>(filter);
            if (value == null || fieldType == null)
                return null;

            fieldType = Nullable.GetUnderlyingType(fieldType) ?? fieldType;
            if (value is JValue jValue)
                value = jValue.Value;
            if (IsDateTime(value, fieldType))
                return value;

            if (IsEnum(fieldType))
            {
                // kd: Dapper treats enums as ints, so we pretend it is string not an enum.
                return ParseEnum(value, fieldType)?.ToString();
            }

            if (IsGuid(fieldType))
            {
                return Guid.TryParse(value.ToString(), out var guidResult)
                    ? (Guid?)guidResult
                    : null;
            }

            if (IsBool(fieldType))
            {
                return Boolean.TryParse(value.ToString(), out var booleanResult)
                    ? (Boolean?)booleanResult
                    : null;
            }

            var intTypes = new[] { typeof(long), typeof(int), typeof(short) };
            var isInt = intTypes.Contains(fieldType) && intTypes.Contains(GetValueType(value));

            if (GetValueType(value) != fieldType && !isInt)
            {
                return null;
            }

            return Convert.ChangeType(value, fieldType);
        }
        catch (InvalidCastException)
        {
            return null;
        }
    }

    private static Type? GetValueType(object? value)
    {
        if(value == null)
        {
            return null;
        }

        var valueType = value.GetType();
        if (value is JValue jValue)
        {
            valueType = jValue.Value!.GetType();
        }

        return valueType;
    }

    public static Type? GetInnerFieldType<TResult>(this FilteringConfiguration.Filter filter)
    {
        if (filter.Field != null)
        {
            var type = GetField<TResult>(filter.Field)?.PropertyType;

            if (type != null) 
                return Nullable.GetUnderlyingType(type) ?? type;
        }

        return null;
    }

    private static Type? GetFieldType<TResult>(FilteringConfiguration.Filter filter)
    {
        return GetField<TResult>(filter.Field!)?.PropertyType;
    }

    public static string? GetFieldName<TResult>(this FilteringConfiguration.Filter filter)
    {
        return GetFieldName<TResult>(filter.Field!);
    }

    public static string? GetFieldName<TResult>(this SortingConfiguration sortingConfiguration)
    {
        return GetFieldName<TResult>(sortingConfiguration.Field!);
    }

    private static string? GetFieldName<TResult>(string fieldName)
    {
        return GetField<TResult>(fieldName)?.Name;
    }

    private static PropertyInfo? GetField<TResult>(string fieldName)
    {
        var type = typeof(TResult).GetGenericArguments().Single();

        return type.GetProperties().SingleOrDefault(f =>
            string.Equals(f.Name, fieldName, StringComparison.CurrentCultureIgnoreCase));
    }

    private static object? ParseEnum(object value, Type fieldType)
    {
        Enum.TryParse(GetNotNullableType(fieldType), value.ToString()!, true, out object? enumValue);
        return enumValue;
    }

    private static bool IsEnum(Type fieldType)
    {
        return GetNotNullableType(fieldType).IsEnum;
    }

    private static bool IsBool(Type fieldType)
    {
        return GetNotNullableType(fieldType) == typeof(bool);
    }

    private static bool IsGuid(Type fieldType)
    {
        return GetNotNullableType(fieldType) == typeof(Guid);
    }

    private static bool IsDateTime(object value, Type fieldType)
    {
        return
            (GetNotNullableType(fieldType) == typeof(DateTimeOffset) || GetNotNullableType(fieldType) == typeof(DateTime)) &&
            (value is DateTimeOffset || value is DateTime);
    }

    private static Type GetNotNullableType(Type fieldType)
    {
        return Nullable.GetUnderlyingType(fieldType) ?? fieldType;
    }
}