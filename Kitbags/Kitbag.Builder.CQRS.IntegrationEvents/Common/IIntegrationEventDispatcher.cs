using System;
using System.Threading.Tasks;
using Kitbag.Builder.MessageBus.IntegrationEvent;

namespace Kitbag.Builder.CQRS.IntegrationEvents.Common;

public interface IIntegrationEventDispatcher
{
    Task SendAsync<T>(
        T @event,
        Type? handlerType = null) where T : class, IIntegrationEvent;
}