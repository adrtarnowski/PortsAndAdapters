namespace Kitbag.Builder.Persistence.DatabaseMigration.Common
{
    public class DatabaseMigrationProperties
    {
        public string? ConnectionString { get; set; }
        public string? ScriptsPath { get; set; }
        public string? SchemaPath { get; set; }
        public string? PreDeploymentFolder { get; set; }
        public string? MigrationFolder { get; set; }
        public string? PostDeploymentFolder { get; set; }
        public string? CustomerSchema { get; set; }
        public string? CreateSchemaPath { get; set; }
    }
}