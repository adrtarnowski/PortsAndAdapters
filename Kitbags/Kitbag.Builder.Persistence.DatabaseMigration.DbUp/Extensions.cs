using System;
using System.IO;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Persistence.DatabaseMigration.Common;
using Kitbag.Builder.Persistence.DatabaseMigration.DbUp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kitbag.Builder.Persistence.DatabaseMigration.DbUp
{
    public static class Extensions
    {
        public static IKitbagBuilder AddDbUp(this IServiceCollection services, Action<ILoggingBuilder> loggingConfig, string appSettingFileName = "db_appsettings.json", string sectionName = "DbUp")
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appSettingFileName, optional: true, reloadOnChange: true).Build();
            services.AddSingleton<IConfiguration>(config);
            services.AddLogging(loggingConfig);

            var builder = new KitbagBuilder(services, config);
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;

            var dbUpProperties = builder.GetSettings<DatabaseMigrationProperties>(sectionName);
            builder.Services.AddSingleton(dbUpProperties);
            builder.Services.AddSingleton<IMigrationService, DbUpMigrationService>();
            builder.Services.AddSingleton<IAutoChangeService, AutoChangeService>();

            return builder;
        }
    }
}