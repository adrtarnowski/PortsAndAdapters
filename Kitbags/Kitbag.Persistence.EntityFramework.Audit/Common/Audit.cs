using System;

namespace Kitbag.Persistence.EntityFramework.Audit.Common
{
    public class Audit
    {
        public long Id { get; set; }
        public string? TableName { get; set; }
        public string? Entity { get; set; }
        public DateTime DateTime { get; set; }
        public string? KeyValues { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? CorrelationId { get; set; }
        public string? ChangeContext { get; set; }
        public AuditChangeType ChangeType { get; set; }
    }
    
    public enum AuditChangeType
    {
        Added = 0,
        Modified = 1,
        Deleted = 2
    }
}