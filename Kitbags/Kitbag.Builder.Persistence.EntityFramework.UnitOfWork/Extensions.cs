using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Persistence.Core.Common;
using Kitbag.Persistence.EntityFramework.UnitOfWork.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Persistence.EntityFramework.UnitOfWork
{
    public static class Extensions
    {
        public static IKitbagBuilder AddUnitOfWork(this IKitbagBuilder builder, string sectionName = "UnitOfWork")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            
            builder.Services.AddScoped<IDomainEventsAccessor, DomainEventsAccessor>();
            builder.Services.AddScoped<IUnitOfWork, Common.UnitOfWork>();
            builder.Services.AddScoped<ITransactionalUnitOfWork, Common.UnitOfWork>();
            return builder;
        }
    }
}