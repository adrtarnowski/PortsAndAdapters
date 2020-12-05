using Microsoft.EntityFrameworkCore;
using TLJ.PortsAndAdapters.Infrastructure.Persistence.Configurations;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        protected internal DbSet<Kitbag.Persistence.EntityFramework.Audit.Common.Audit>? Audits { get; set; }
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AuditConfiguration).Assembly);
        }
    }
}