using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Kitbag.Builder.Persistence.DatabaseMigration.Common
{
public class AutoChangeService : IAutoChangeService
    {
        private class AutoChange
        {
            public string DirectoryName { get; set; } = string.Empty;
            public string SqlEntitySyntax { get; set; } = string.Empty;
        }

        private readonly List<AutoChange> AutoChanges = new List<AutoChange>
        {
            new AutoChange{ DirectoryName = "Functions", SqlEntitySyntax = "FUNCTION" },
            new AutoChange{ DirectoryName = "Views", SqlEntitySyntax = "VIEW" },
            new AutoChange{ DirectoryName = "Stored Procedures", SqlEntitySyntax = "PROCEDURE" },
        };

        public string AutoChangeScriptName => "AutoChanges.sql";

        private readonly DatabaseMigrationProperties _dbUpProperties;
        private readonly ILogger<AutoChangeService> _logger;

        public AutoChangeService(DatabaseMigrationProperties dbUpProperties, ILogger<AutoChangeService> logger)
        {
            _dbUpProperties = dbUpProperties;
            _logger = logger;
            if(_dbUpProperties.SchemaPath == null)
                throw new ArgumentException("Schema path must be defined");
        }

        public string GetAutoChangeScript()
        {
            var automaticChangeDirs = GetAutoChangeDirs();
            var sqlScripts = new List<string>();

            foreach (var d in automaticChangeDirs)
            {
                var autoChange = d.Item1;
                DirectoryInfo di = d.Item2;
                _logger.LogInformation(
                    $"Getting automatic changes on " +
                    $"{Directory.GetParent(di.FullName)!.Name}.{di.Name}");
                sqlScripts.AddRange(GetUpgradeScriptsOnAutoChangeEntities(autoChange, di));
            }
            
            var autoChangeScript = string.Join(Environment.NewLine, sqlScripts);
            return autoChangeScript;
        }

        private IEnumerable<string> GetUpgradeScriptsOnAutoChangeEntities(AutoChange autoChange, DirectoryInfo di)
        {
            var sqlFiles = di.GetFiles("*.sql", SearchOption.AllDirectories).Select(x => x.FullName);

            foreach (var sqlFile in sqlFiles)
            {
                var fileContent = File.ReadAllText(sqlFile);
                fileContent = Regex.Replace(
                    fileContent,
                    string.Format(@"CREATE\s+{0}", autoChange.SqlEntitySyntax),
                    string.Format("GO{0}CREATE OR ALTER {1}", Environment.NewLine, autoChange.SqlEntitySyntax),
                    RegexOptions.IgnoreCase);

                yield return fileContent;
            }
        }

        private List<(AutoChange, DirectoryInfo)> GetAutoChangeDirs()
        {
            var schemaLocationDir = new DirectoryInfo(_dbUpProperties.SchemaPath!);
            List<(AutoChange, DirectoryInfo)> autoChangeDirs = new List<(AutoChange, DirectoryInfo)>();

            foreach (var ac in AutoChanges)
            {
                foreach (var dir in schemaLocationDir.GetDirectories(ac.DirectoryName, SearchOption.AllDirectories))
                {
                    autoChangeDirs.Add((ac, dir));
                }
            }
            
            return autoChangeDirs;
        }
    }
}