using Kitbag.Builder.Persistence.DatabaseMigration.Common;
using Kitbag.Builder.Persistence.DatabaseMigration.DbUp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TLJ.PortsAndAdapters.DatabaseMigration
{
    public class Program
    {
        static int Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddDbUp(c => c.AddConsole())
                .Build();

            var dbUpService = serviceProvider.GetService<IMigrationService>();

            if (!dbUpService.ExecuteMigrationScripts())
                return -1;

            return 0;
        }
    }
}