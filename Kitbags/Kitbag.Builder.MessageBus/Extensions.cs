using System;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;

namespace Kitbag.Builder.MessageBus
{
    public static class Extensions
    {
        public static string GetLabelFor<T>()
            where T : IIntegrationEvent
        {
            return GetLabelForType(typeof(T));
        }

        public static string GetLabel(this IIntegrationEvent integrationEvent)
        {
            return GetLabelForType(integrationEvent.GetType());
        }

        private static string GetLabelForType(Type type)
        {
            var eventTypeName = type.Name.Replace("IntegrationEvent", "");
            var eventName = eventTypeName.Underscore().ToLower();

            return $"{eventName}";
        }
    }
}