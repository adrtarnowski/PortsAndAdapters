using System;
using Kitbag.Builder.Core.Common;

namespace Kitbag.Builder.Core.Domain
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTimeOffset OccuredOn { get; } = SystemTime.OffsetNow();
    }
}