using System;

namespace Kitbag.Builder.Persistence.Core.Common.Logs;

public class AuditTrail
{
    public long Id { get; set; }
    public string? TableName { get; set; }
    public string? Entity { get; set; }
    public DateTime DateTime { get; set; }
    public string? KeyValues { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public AuditTrailChangeType ChangeType { get; set; }
}
    
public enum AuditTrailChangeType
{
    Added = 0,
    Modified = 1,
    Deleted = 2
}