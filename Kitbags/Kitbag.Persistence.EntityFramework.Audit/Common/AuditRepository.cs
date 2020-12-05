using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kitbag.Persistence.EntityFramework.Audit.Common
{
    public class AuditRepository<TContext>  where TContext : DbContext
    {
        private readonly TContext _context;

        public AuditRepository(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));;
        }

        public Task AddRangeAsync(List<Audit> audits)
        {
            return _context.AddRangeAsync(audits);
        }
    }
}