using System;
using System.Collections.Generic;
using Kitbag.Builder.MessageBus.IntegrationEvent;

namespace Kitbag.Builder.MessageBus.ServiceBus.Common
{
    public class BusSubscriptionManager : IBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<Type>> _handlers = new Dictionary<string, List<Type>>();
        private readonly Dictionary<string, Type> _eventTypes = new Dictionary<string, Type>();

        public void AddSubscription<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventLabel = GetEventLabel<T>();

            if (!HasSubscriptionsForEvent(eventLabel)) 
                _handlers.Add(eventLabel, new List<Type>());
            
            if (_handlers[eventLabel].Contains(typeof(TH)))
                throw new ArgumentException(
                    $"Handler Type {typeof(TH).Name} already registered for '{eventLabel}'");
            
            _handlers[eventLabel].Add(typeof(TH));
            if (!_eventTypes.ContainsKey(eventLabel)) 
                _eventTypes.Add(eventLabel, typeof(T));
        }

        public bool HasSubscriptionsForEvent<T>() where T : IIntegrationEvent
        {
            var key = GetEventLabel<T>();
            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public string GetEventLabel<T>()
            where T : IIntegrationEvent
        {
            return MessageBus.Extensions.GetLabelFor<T>();
        }


        public IEnumerable<Type> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public Type GetEventTypeByName(string eventName) => _eventTypes[eventName];
    }
}