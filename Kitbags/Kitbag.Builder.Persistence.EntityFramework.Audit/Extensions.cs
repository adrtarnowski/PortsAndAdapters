using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Persistence.Core.Common.Logs;
using Kitbag.Persistence.EntityFramework.Audit.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Persistence.EntityFramework.Audit
{
    public static class Extensions
    {
        public static IKitbagBuilder AddEntityFrameworkAuditTrail<TDbContext>(this IKitbagBuilder builder,
            string sectionName = "AuditTrail") where TDbContext : DbContext
        {
            if (!builder.TryRegisterKitBag(sectionName)) 
                return builder;
           
            builder.Services.AddScoped<IAuditTrailRepository, AuditTrailRepository<TDbContext>>();
            builder.Services.AddScoped<IAuditTrailProvider, AuditTrailProvider<TDbContext>>();
            return builder;
        }
    }
}