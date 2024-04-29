using System.Text.RegularExpressions;
using DbUp.Engine;

namespace Kitbag.Builder.Persistence.DatabaseMigration.DbUp.Services;

public class DbUpPreprocessor : IScriptPreprocessor
{
    private readonly string _dbUpPropertiesCustomerSchema;

    public DbUpPreprocessor(string dbUpPropertiesCustomerSchema)
    {
        _dbUpPropertiesCustomerSchema = dbUpPropertiesCustomerSchema;
    }

    public string Process(string contents)
    {
        var script = !string.IsNullOrEmpty(_dbUpPropertiesCustomerSchema)
            ? !contents.Contains("IF NOT EXISTS") ? Regex.Replace(
                contents,
                "dbo",
                _dbUpPropertiesCustomerSchema,
                RegexOptions.IgnoreCase) : contents
            : contents;

        return script;
    }
}