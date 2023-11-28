namespace Kitbag.Builder.Outbox.Common;

public interface IOutbox
{
    void Add(OutboxMessage outbox);
}