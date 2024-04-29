using System;

namespace Kitbag.Builder.MessageBus.IntegrationEvent;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTimeOffset CreationDate { get; }
}