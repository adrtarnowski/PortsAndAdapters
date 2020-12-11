namespace Kitbag.Builder.Persistence.DatabaseMigration.Common
{
    public interface IMigrationService
    {
        bool ExecuteMigrationScripts();
    }
}