using System;
using System.Linq;

namespace Kitbag.Builder.Core.Common;

public static class StringExtensions
{
    public static string? IfNotNullOrWhiteSpace(this string? obj, Func<string, string> then)
    {
        if (!string.IsNullOrWhiteSpace(obj)) 
            return then(obj!);
        return null;
    }

    public static string? FirstCharToUpper(this string? input) =>
        input switch
        {
            null => null,
            "" => "",
            _ => input.First().ToString().ToUpper() + input.Substring(1)
        };

    public static string? FirstCharToLower(this string? input) =>
        input switch
        {
            null => null,
            "" => "",
            _ => input.First().ToString().ToLower() + input.Substring(1)
        };
        
    public static T ToEnum<T>(this string value, T defaultValue) where T : struct
    {
        if (string.IsNullOrEmpty(value)) 
            return defaultValue;
        T result;
        return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
    }

    public static T? ToEnum<T>(this string? value) where T : struct
    {
        if (Enum.TryParse<T>(value, true, out T result)) 
            return result;
        return null;
    }
        
    public static string Underscore(this string value)
    {
        return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()));
    }
}