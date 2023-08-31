using System;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;

namespace Kitbag.Builder.MessageBus
{
    public static class Extensions
    {
        public static string GetEventFor<T>()
            where T : IIntegrationEvent
        {
            return GetEventForType(typeof(T));
        }

        public static string GetEventType(this IIntegrationEvent integrationEvent)
        {
            return GetEventForType(integrationEvent.GetType());
        }

        private static string GetEventForType(Type type)
        {
            var eventTypeName = type.Name.Replace("IntegrationEvent", "");
            var eventName = eventTypeName.Underscore().ToLower();

            return $"{eventName}";
        }
    }
}