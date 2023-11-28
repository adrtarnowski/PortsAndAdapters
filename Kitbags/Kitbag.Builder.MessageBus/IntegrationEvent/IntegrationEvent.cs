using System;
using Kitbag.Builder.Core.Common;

namespace Kitbag.Builder.MessageBus.IntegrationEvent
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        public Guid Id { get; protected set; }

        public DateTimeOffset CreationDate { get; protected set; }

        protected IntegrationEvent(Guid id, DateTimeOffset creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }

        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = SystemTime.Now();
        }
    }
}