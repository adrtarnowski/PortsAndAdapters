using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kitbag.Builder.Persistence.Core.Common.Logs;
using Microsoft.EntityFrameworkCore;

namespace Kitbag.Persistence.EntityFramework.Audit.Common;

public class AuditTrailRepository<TContext> : IAuditTrailRepository  where TContext : DbContext
{
    private readonly TContext _context;

    public AuditTrailRepository(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));;
    }

    public Task AddRangeAsync(List<AuditTrail> audits)
    {
        return _context.AddRangeAsync(audits);
    }
}