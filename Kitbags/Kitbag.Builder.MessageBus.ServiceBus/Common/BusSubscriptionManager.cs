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
            var eventSubject = GetEventSubject<T>();

            if (!HasSubscriptionsForEvent(eventSubject)) 
                _handlers.Add(eventSubject, new List<Type>());
            
            if (_handlers[eventSubject].Contains(typeof(TH)))
                throw new ArgumentException(
                    $"Handler Type {typeof(TH).Name} already registered for '{eventSubject}'");
            
            _handlers[eventSubject].Add(typeof(TH));
            if (!_eventTypes.ContainsKey(eventSubject)) 
                _eventTypes.Add(eventSubject, typeof(T));
        }

        public bool HasSubscriptionsForEvent<T>() where T : IIntegrationEvent
        {
            var key = GetEventSubject<T>();
            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public string GetEventSubject<T>()
            where T : IIntegrationEvent
        {
            return MessageBus.Extensions.GetEventFor<T>();
        }

        public IEnumerable<Type> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public Type GetEventTypeByName(string eventName) => _eventTypes[eventName];
    }
}