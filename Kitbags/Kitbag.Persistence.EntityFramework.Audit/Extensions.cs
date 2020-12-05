using Kitbag.Builder.Core.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kitbag.Persistence.EntityFramework.Audit
{
    public static class Extensions
    {
        public static IKitbagBuilder AddEntityFrameworkAuditTrail<TDbContext>(this IKitbagBuilder builder,
            string sectionName = "AuditTrail") where TDbContext : DbContext
        {
            if (!builder.TryRegisterKitBag(sectionName)) 
                return builder;
            
            return builder;
        }
    }
}