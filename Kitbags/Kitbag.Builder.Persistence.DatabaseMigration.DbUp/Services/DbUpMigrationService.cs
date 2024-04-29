using System.IO;
using DbUp;
using DbUp.Engine;
using DbUp.Support;
using Kitbag.Builder.Persistence.DatabaseMigration.Common;
using Microsoft.Extensions.Logging;

namespace Kitbag.Builder.Persistence.DatabaseMigration.DbUp.Services;

public class DbUpMigrationService : IMigrationService
{
    private readonly IAutoChangeService _autoChangeService;
    private readonly DatabaseMigrationProperties _databaseMigrationProperties;
    private readonly ILogger<DbUpMigrationService> _logger;
        
    public DbUpMigrationService(
        IAutoChangeService autoChangeService, 
        DatabaseMigrationProperties databaseMigrationProperties, 
        ILogger<DbUpMigrationService> logger)
    {
        _autoChangeService = autoChangeService;
        _databaseMigrationProperties = databaseMigrationProperties;
        _logger = logger;
    }

    public bool ExecuteMigrationScripts()
    {
        _logger.LogInformation("Start executing migration scripts...");
        var autoChangeScript = _autoChangeService.GetAutoChangeScript();

        if (_databaseMigrationProperties.ScriptsPath != null &&
            _databaseMigrationProperties.PreDeploymentFolder != null &&
            _databaseMigrationProperties.MigrationFolder != null &&
            _databaseMigrationProperties.PostDeploymentFolder != null &&
            _databaseMigrationProperties.CustomerSchema != null)
        {
            var preDeploymentScriptsPath = Path.Combine(_databaseMigrationProperties.ScriptsPath, _databaseMigrationProperties.PreDeploymentFolder);
            var migrationScriptsPath = Path.Combine(_databaseMigrationProperties.ScriptsPath, _databaseMigrationProperties.MigrationFolder);
            var postdeploymentScriptsPath = Path.Combine(_databaseMigrationProperties.ScriptsPath, _databaseMigrationProperties.PostDeploymentFolder);

            var newSchema = _databaseMigrationProperties.CustomerSchema;

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(_databaseMigrationProperties.ConnectionString);

            upgrader
                .WithScriptsFromFileSystem(preDeploymentScriptsPath, new SqlScriptOptions
                {
                    ScriptType = ScriptType.RunAlways,
                    RunGroupOrder = 0
                })
                .WithScriptsFromFileSystem(migrationScriptsPath, new SqlScriptOptions
                {
                    ScriptType = ScriptType.RunOnce,
                    RunGroupOrder = 1
                })
                .WithScript(_autoChangeService.AutoChangeScriptName, autoChangeScript, new SqlScriptOptions
                {
                    ScriptType = ScriptType.RunAlways,
                    RunGroupOrder = 2
                })
                .WithScriptsFromFileSystem(postdeploymentScriptsPath, new SqlScriptOptions
                {
                    ScriptType = ScriptType.RunAlways,
                    RunGroupOrder = 3
                })
                .LogToAutodetectedLog()
                .WithTransaction();

            var result = upgrader.Build().PerformUpgrade();

            if (!string.IsNullOrEmpty(newSchema))
            {
                upgrader
                    .WithVariable("schema", newSchema)
                    .WithScript("CreateSchema.sql", File.ReadAllText(_databaseMigrationProperties.CreateSchemaPath), new SqlScriptOptions
                    {
                        ScriptType = ScriptType.RunAlways,
                        RunGroupOrder = 0
                    })
                    .WithScriptsFromFileSystem(preDeploymentScriptsPath, new SqlScriptOptions
                    {
                        ScriptType = ScriptType.RunAlways,
                        RunGroupOrder = 1
                    })
                    .WithPreprocessor(new DbUpPreprocessor(_databaseMigrationProperties.CustomerSchema))
                    .WithScriptsFromFileSystem(migrationScriptsPath, new SqlScriptOptions
                    {
                        ScriptType = ScriptType.RunAlways,
                        RunGroupOrder = 2
                    })
                    .WithScript(_autoChangeService.AutoChangeScriptName, autoChangeScript, new SqlScriptOptions
                    {
                        ScriptType = ScriptType.RunAlways,
                        RunGroupOrder = 3
                    })
                    .WithScriptsFromFileSystem(postdeploymentScriptsPath, new SqlScriptOptions
                    {
                        ScriptType = ScriptType.RunAlways,
                        RunGroupOrder = 4
                    })
                    .LogToAutodetectedLog()
                    .WithTransaction();

                result = upgrader.Build().PerformUpgrade();
            }

            if (!result.Successful)
            {
                _logger.LogError(result.Error.ToString());
                return false;
            }
        }

        _logger.LogInformation("Success!");
        return true;
    }
}