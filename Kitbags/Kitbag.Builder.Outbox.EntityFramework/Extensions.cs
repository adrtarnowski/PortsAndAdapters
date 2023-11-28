using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Outbox.Common;
using Kitbag.Builder.Outbox.EntityFramework.Common;
using Kitbag.Builder.Outbox.Schedulers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.Outbox.EntityFramework
{
    public static class Extensions
    {
        public static IKitbagBuilder AddEntityFrameworkOutbox<TDbContext>(this IKitbagBuilder builder, string sectionName = "Outbox") where TDbContext : DbContext
        {
            if (!builder.TryRegisterKitBag(sectionName)) 
                return builder;
            
            builder.Services.AddScoped<IOutBoxRepository, OutBoxRepository<TDbContext>>();
            builder.Services.AddScoped<IDomainEventScheduler, DomainEventScheduler>();
            builder.Services.AddScoped<IOutboxEventDispatcher, OutboxEventDispatcher>();
            
            return builder;
        }
    }
}