using System;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.CQRS.Core;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.CQRS.IntegrationEvents;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Kitbag.Builder.MessageBus.ServiceBus;
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

            builder.AddCQRSIntegrationEvents();
            //TODO: Verify if works
            //builder.AddServiceBus();
            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
        {
            var busSubscriber = builder.ApplicationServices.GetService<IEventBusSubscriber>();
            //TODO: Verify if works
            //busSubscriber.Subscribe<X, Y>();
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