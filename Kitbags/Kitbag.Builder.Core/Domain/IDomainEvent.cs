using System;

namespace Kitbag.Builder.Core.Domain
{
    public interface IDomainEvent
    {
        DateTime OccuredOn { get; }
    }
}