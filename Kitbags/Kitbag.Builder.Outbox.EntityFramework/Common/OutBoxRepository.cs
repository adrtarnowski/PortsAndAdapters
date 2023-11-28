using Kitbag.Builder.Outbox.Common;
using Microsoft.EntityFrameworkCore;

namespace Kitbag.Builder.Outbox.EntityFramework.Common;

public class OutBoxRepository<TContext> : IOutBoxRepository where TContext : DbContext
{
    private readonly TContext _context;

    public OutBoxRepository(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));;
    }

    public async Task AddRange(List<OutboxMessage> messages)
    {
        await _context.AddRangeAsync(messages);
    }
}