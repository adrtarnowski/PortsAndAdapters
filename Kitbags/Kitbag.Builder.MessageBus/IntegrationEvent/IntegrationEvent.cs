using System;
using Kitbag.Builder.Core.Common;

namespace Kitbag.Builder.MessageBus.IntegrationEvent
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        public Guid Id { get; }

        public DateTime CreationDate { get; }

        public IntegrationEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = SystemTime.Now();
        }
    }
}