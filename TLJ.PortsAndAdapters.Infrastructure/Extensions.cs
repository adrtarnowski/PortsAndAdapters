using System;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.CQRS.Core;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.CQRS.Dapper;
using Kitbag.Builder.CQRS.IntegrationEvents;
using Kitbag.Builder.Logging.AppInsights.Decorators;
using Kitbag.Builder.MessageBus.IntegrationEvent;
using Kitbag.Builder.MessageBus.ServiceBus;
using Kitbag.Builder.Persistence.EntityFramework;
using Kitbag.Builder.RunningContext;
using Kitbag.Builder.WebApi.Common;
using Kitbag.Persistence.EntityFramework.Audit;
using Kitbag.Persistence.EntityFramework.Audit.Common;
using Kitbag.Persistence.EntityFramework.UnitOfWork;
using Kitbag.Persistence.EntityFramework.UnitOfWork.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TLJ.PortsAndAdapters.Application.Bookmaking.Events;
using TLJ.PortsAndAdapters.Infrastructure.Persistence;
using TLJ.PortsAndAdapters.Infrastructure.Persistence.Repositories;
using TLJ.PortsAndAdapters.Infrastructure.Events;
using TLJ.PortsAndAdapters.Infrastructure.ReadModel;

namespace TLJ.PortsAndAdapters.Infrastructure
{
    public static class Extensions
    {
        public static IKitbagBuilder AddInfrastructure(this IKitbagBuilder builder)
        {
            builder.AddEntityFramework<DatabaseContext>();
            builder.AddEntityFrameworkAuditTrail<DatabaseContext>();
            builder.AddUnitOfWork();
           
            builder.AddCQRS();
            builder.AddCQRSIntegrationEvents();
            builder.AddDapperForQueries(new DapperInitializer());
            
            builder.Services.Decorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
            builder.Services.Decorate(typeof(ICommandHandler<>), typeof(AuditTrailCommandHandlerDecorator<>));
            builder.Services.Decorate(typeof(ICommandHandler<>), typeof(AppInsightLoggingCommandHandlerDecorator<>));
            
            builder.Services.RegisterRepositories();
            builder.AddRunningContext(x => x.GetService<IHttpRunningContextProvider>());
            
            // ServiceBus register events
            builder.AddServiceBus();
            builder.AddServiceBusPublisher<CloseBookmakingEvent>();
            builder.AddServiceBusSubscriber<ServiceBusSubscriptionRegistrationInitializer>();
            builder.AddServiceBusWorker();
            
            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
        {
            // ServiceBus register event example
            var busSubscriber = builder.ApplicationServices.GetRequiredService<IEventSubscriber>();
            busSubscriber.Subscribe<CloseBookmakingEvent, CloseBookmakingEventHandler>();
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