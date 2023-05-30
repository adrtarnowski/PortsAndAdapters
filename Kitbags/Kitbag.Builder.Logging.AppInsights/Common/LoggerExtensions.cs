using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kitbag.Builder.Logging.AppInsights.Clients;
using Microsoft.Extensions.Logging;

namespace Kitbag.Builder.Logging.AppInsights.Common
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Example usage:
        /// <para>BeginScopeWith("userId", "correlationId")</para>
        /// <para>BeginScopeWith("userId", "correlationId", new { propertyA = "valueA", propertyB = "valueB" })</para>
        /// </summary>
        public static IDisposable BeginScopeWith(this ILogger logger, string userId, string correlationId,
            object? values = null)
        {
            var dictionary = GetValuesAsDictionary(values);
            dictionary.Add(LoggingConstants.CorrelationId, correlationId);
            dictionary.Add(LoggingConstants.UserId, userId);

            return logger.BeginScope(dictionary);
        }

        /// <summary>
        /// Example usage:
        /// <para>BeginScopeWith("userId")</para>
        /// <para>BeginScopeWith("userId", new { propertyA = "valueA", propertyB = "valueB" })</para>
        /// </summary>
        public static IDisposable BeginScopeWith(this ILogger logger, string userId, object? values = null)
        {
            var dictionary = GetValuesAsDictionary(values);
            dictionary.Add(LoggingConstants.UserId, userId);

            return logger.BeginScope(dictionary);
        }

        private static Dictionary<string, object> GetValuesAsDictionary(object? values)
        {
            var valuesAsDictionary = values as Dictionary<string, object>;

            if (valuesAsDictionary != null)
            {
                return valuesAsDictionary;
            }

            valuesAsDictionary = new Dictionary<string, object>();

            if (values != null)
            {
                IEnumerable<PropertyInfo> objectProperties = values.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => prop.GetIndexParameters().Length == 0 && prop.GetMethod != null);

                foreach (var property in objectProperties)
                {
                    valuesAsDictionary.Add(property.Name, property.GetValue(values) ?? string.Empty);
                }
            }

            return valuesAsDictionary;
        }
    }
}