using Kitbag.Builder.Core.Common;
using Kitbag.Builder.Core.Domain;
using Kitbag.Builder.Outbox.Common;
using Kitbag.Builder.Outbox.Schedulers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Kitbag.Builder.Outbox.EntityFramework.Common;

public class DomainEventScheduler : IDomainEventScheduler
{
    private readonly IOutBoxRepository _outbox;
    private readonly Guid _batchId;

    public DomainEventScheduler(
        IOutBoxRepository outbox,
        DbContext context)
    {
        _outbox = outbox;
        _batchId = context.Database.CurrentTransaction?.TransactionId ?? Guid.NewGuid();
    }

    public async Task Enqueue(IDomainEvent domainEvent)
    {
        var outboxMessage = new OutboxMessage
        {
            CreationDate = SystemTime.Now(),
            Type = domainEvent.GetType().AssemblyQualifiedName,
            BatchId = _batchId,
            Payload = JsonConvert.SerializeObject(domainEvent),
            Discriminator = OutboxMessageDiscriminator.Event,
            Id = Guid.NewGuid()
        };

        await _outbox.AddRange(new List<OutboxMessage>{ outboxMessage });
    }
}