using System;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.CQRS.Core;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.Persistence.EntityFramework;
using Kitbag.Persistence.EntityFramework.Audit;
using Kitbag.Persistence.EntityFramework.Audit.Common;
using Kitbag.Persistence.EntityFramework.UnitOfWork;
using Kitbag.Persistence.EntityFramework.UnitOfWork.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TLJ.PortsAndAdapters.Infrastructure.Persistence;
using TLJ.PortsAndAdapters.Infrastructure.Persistence.Repositories;

namespace TLJ.PortsAndAdapters.Infrastructure
{
    public static class Extensions
    {
        public static IKitbagBuilder AddInfrastructure(this IKitbagBuilder builder)
        {
            builder.AddCQRS();
            builder.AddEntityFramework<DatabaseContext>();
            builder.AddEntityFrameworkAuditTrail<DatabaseContext>();
            builder.AddUnitOfWork();
            builder.Services.Decorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
            builder.Services.Decorate(typeof(ICommandHandler<>), typeof(AuditTrailCommandHandlerDecorator<>));
            builder.Services.RegisterRepositories();
            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
        {
            return builder;
        }
        
        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.Scan(s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => 
                        c.AssignableTo(typeof(DatabaseRepository<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        }
    }
}