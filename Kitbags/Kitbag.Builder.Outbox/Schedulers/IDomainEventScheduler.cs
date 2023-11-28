using Kitbag.Builder.Core.Domain;

namespace Kitbag.Builder.Outbox.Schedulers;

public interface IDomainEventScheduler
{
    Task Enqueue(IDomainEvent domainEvent);
}