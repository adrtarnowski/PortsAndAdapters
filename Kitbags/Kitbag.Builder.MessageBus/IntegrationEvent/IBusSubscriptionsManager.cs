using System;
using System.Collections.Generic;

namespace Kitbag.Builder.MessageBus.IntegrationEvent
{
    public interface IBusSubscriptionsManager
    {
        void AddSubscription<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;
        bool HasSubscriptionsForEvent<T>() where T : IIntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);
        IEnumerable<Type> GetHandlersForEvent(string eventName);
        public Type GetEventTypeByName(string eventName);
        string GetEventSubject<T>() where T : IIntegrationEvent;
    }
}