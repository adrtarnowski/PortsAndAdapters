using System;
using Kitbag.Builder.Core.Common;

namespace Kitbag.Builder.MessageBus.IntegrationEvent
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        public Guid Id { get; protected set; }

        public DateTime CreationDate { get; protected set; }

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