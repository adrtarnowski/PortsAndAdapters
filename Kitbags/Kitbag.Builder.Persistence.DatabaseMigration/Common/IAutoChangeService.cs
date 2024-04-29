namespace Kitbag.Builder.Persistence.DatabaseMigration.Common;

public interface IAutoChangeService
{
    string AutoChangeScriptName { get; }
    string GetAutoChangeScript();
}