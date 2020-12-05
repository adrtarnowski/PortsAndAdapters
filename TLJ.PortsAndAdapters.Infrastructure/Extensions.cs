using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.CQRS.Core;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.Persistence.EntityFramework;
using Kitbag.Persistence.EntityFramework.UnitOfWork;
using Kitbag.Persistence.EntityFramework.UnitOfWork.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TLJ.PortsAndAdapters.Infrastructure.Persistence;

namespace TLJ.PortsAndAdapters.Infrastructure
{
    public static class Extensions
    {
        public static IKitbagBuilder AddInfrastructure(this IKitbagBuilder builder)
        {
            builder.AddCQRS();
            builder.AddEntityFramework<DatabaseContext>();
            builder.AddUnitOfWork();
            builder.Services.Decorate(
                typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
        {
            return builder;
        }
    }
}