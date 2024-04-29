using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kitbag.Builder.Core.Common;
using Kitbag.Builder.Persistence.Core.Common.Logs;
using Microsoft.EntityFrameworkCore;

namespace Kitbag.Persistence.EntityFramework.Audit.Common;

public class AuditTrailProvider<TContext> : IAuditTrailProvider where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IAuditTrailRepository _auditTrailRepository;

    private static readonly IReadOnlyDictionary<EntityState, AuditTrailChangeType> _stateMapping
        = new Dictionary<EntityState, AuditTrailChangeType>
        {
            {EntityState.Added, AuditTrailChangeType.Added},
            {EntityState.Modified, AuditTrailChangeType.Modified},
            {EntityState.Deleted, AuditTrailChangeType.Deleted}
        };

    public AuditTrailProvider(
        TContext context, 
        IAuditTrailRepository auditTrailRepository)
    {
        _context = context;
        _auditTrailRepository = auditTrailRepository;
    }

    public async Task LogChangesAsync()
    {
        var currentDate = SystemTime.Now();
        List<AuditTrail> audits = new List<AuditTrail>();
        var auditBuilder = new AuditBuilder();
        var entries = _context.ChangeTracker.Entries()
            .Where(e =>
                e.State != EntityState.Detached && e.State != EntityState.Unchanged);

        foreach (var entry in entries)
        {
            auditBuilder
                .SetTableName(entry.Metadata.GetTableName())
                .SetEntityName(entry.Entity.ToString()!)
                .SetAuditDate(currentDate)
                .SetChangeType(_stateMapping[entry.State]);

            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                    auditBuilder.AddPrimaryKey(propertyName, property.CurrentValue);
                    
                if (entry.State == EntityState.Added)
                    auditBuilder.NewValue(propertyName, property.CurrentValue);

                if (entry.State == EntityState.Deleted)
                    auditBuilder.OldValue(propertyName, property.OriginalValue);

                if (entry.State == EntityState.Modified)
                {
                    if (property.IsModified)
                    {
                        auditBuilder.OldValue(propertyName, property.OriginalValue)
                            .NewValue(propertyName, property.CurrentValue);
                    }
                }
            }

            var audit = auditBuilder.Build();
            if (audit != null)
                audits.Add(audit);
        }
        await _auditTrailRepository.AddRangeAsync(audits);
    }
}