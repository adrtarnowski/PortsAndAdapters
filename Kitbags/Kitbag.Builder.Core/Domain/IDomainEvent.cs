using System;

namespace Kitbag.Builder.Core.Domain
{
    public interface IDomainEvent
    {
        DateTimeOffset OccuredOn { get; }
    }
}