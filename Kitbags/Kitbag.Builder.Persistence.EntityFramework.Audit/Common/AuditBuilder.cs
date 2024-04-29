using System;
using System.Collections.Generic;
using Kitbag.Builder.Persistence.Core.Common.Logs;
using Newtonsoft.Json;

namespace Kitbag.Persistence.EntityFramework.Audit.Common;

internal class AuditBuilder
{
    private string? _tableName;
    private string? _entityName;
    private DateTime _auditDate;
    private AuditTrailChangeType _changeType;
    private readonly Dictionary<string, object> _keyValues;
    private readonly Dictionary<string, object> _newValues;
    private readonly Dictionary<string, object> _oldValues;

    public AuditBuilder()
    {
        _keyValues = new Dictionary<string, object>();
        _newValues = new Dictionary<string, object>();
        _oldValues = new Dictionary<string, object>();
    }

    public AuditBuilder SetTableName(string tableName)
    {
        _tableName = tableName;
        return this;
    }

    public AuditBuilder SetEntityName(string entityName)
    {
        _entityName = entityName;
        return this;
    }

    public AuditBuilder AddPrimaryKey(string propertyName, object propertyValue)
    {
        _keyValues[propertyName] = propertyValue;
        return this;
    }

    public AuditBuilder NewValue(string propertyName, object propertyValue)
    {
        _newValues[propertyName] = propertyValue;
        return this;
    }

    public AuditBuilder OldValue(string propertyName, object propertyValue)
    {
        _oldValues[propertyName] = propertyValue;
        return this;
    }

    public AuditBuilder SetAuditDate(DateTime dateTime)
    {
        _auditDate = dateTime;
        return this;
    }

    public AuditBuilder SetChangeType(AuditTrailChangeType changeType)
    {
        _changeType = changeType;
        return this;
    }

    public AuditTrail? Build()
    {
        if (_oldValues.Count == 0
            && _newValues.Count == 0
            && _changeType == AuditTrailChangeType.Modified)
        {
            return null;
        }

        var audit = new AuditTrail();
        audit.TableName = _tableName;
        audit.DateTime = _auditDate;
        audit.KeyValues = JsonConvert.SerializeObject(_keyValues);
        audit.OldValues = _oldValues.Count == 0 ? null : JsonConvert.SerializeObject(_oldValues);
        audit.NewValues = _newValues.Count == 0 ? null : JsonConvert.SerializeObject(_newValues);
        audit.Entity = _entityName;
        audit.ChangeType = _changeType;
        return audit;
    }
}