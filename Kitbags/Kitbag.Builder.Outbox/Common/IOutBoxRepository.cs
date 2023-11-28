namespace Kitbag.Builder.Outbox.Common;

public interface IOutBoxRepository
{
    public Task AddRange(List<OutboxMessage> messages);
}