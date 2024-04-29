using System;
using System.Threading.Tasks;
using Kitbag.Builder.CQRS.IntegrationEvents.Common;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.CQRS.IntegrationEvents.Dispatchers;

public class IntegrationEventDispatcher: IIntegrationEventDispatcher
{
    private readonly IServiceScopeFactory _serviceFactory;

    public IntegrationEventDispatcher(IServiceScopeFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    public async Task SendAsync<T>(T @event, Type? handlerType = null) 
        where T : class, IIntegrationEvent
    {
        using var scope = _serviceFactory.CreateScope();
        handlerType ??= typeof(IIntegrationEventHandler<>).MakeGenericType(@event.GetType());
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        await handler.HandleAsync((dynamic)@event);
    }
}