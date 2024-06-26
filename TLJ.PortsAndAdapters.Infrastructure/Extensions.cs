﻿using System;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.Core.Domain.Exceptions;
using Kitbag.Builder.CQRS.Core;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.CQRS.Dapper;
using Kitbag.Builder.CQRS.IntegrationEvents;
using Kitbag.Builder.Logging.OpenTelemetry.Decorators;
using Kitbag.Builder.MessageBus.ServiceBus;
using Kitbag.Builder.Outbox.EntityFramework;
using Kitbag.Builder.Outbox.EntityFramework.Common;
using Kitbag.Builder.Persistence.EntityFramework;
using Kitbag.Builder.Redis;
using Kitbag.Builder.RunningContext;
using Kitbag.Builder.WebApi;
using Kitbag.Builder.WebApi.Exceptions.Types;
using Kitbag.Builder.WebApi.RunningContext;
using Kitbag.Persistence.EntityFramework.Audit;
using Kitbag.Persistence.EntityFramework.Audit.Common;
using Kitbag.Persistence.EntityFramework.UnitOfWork;
using Kitbag.Persistence.EntityFramework.UnitOfWork.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TLJ.PortsAndAdapters.Application.User.Events;
using TLJ.PortsAndAdapters.Infrastructure.Persistence;
using TLJ.PortsAndAdapters.Infrastructure.Persistence.Repositories;
using TLJ.PortsAndAdapters.Infrastructure.ReadModel;

namespace TLJ.PortsAndAdapters.Infrastructure;

public static class Extensions
{
    public static IKitbagBuilder AddInfrastructure(this IKitbagBuilder builder)
    {
        // Defining database context
        builder.AddEntityFramework<DatabaseContext>();
        builder.AddEntityFrameworkAuditTrail<DatabaseContext>();
        builder.AddEntityFrameworkOutbox<DatabaseContext>();
        builder.AddUnitOfWork();
        builder.AddRedisCacheIntegration();
        builder.Services.RegisterRepositories();
            
        // Defining CQRS
        builder.AddCQRS();
        builder.AddCQRSIntegrationEvents();
        builder.AddDapperForQueries(new DapperInitializer());
            
        // Defining decorators
        builder.Services.Decorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        builder.Services.Decorate(typeof(ICommandHandler<>), typeof(AuditTrailCommandHandlerDecorator<>));
        builder.Services.Decorate(typeof(ICommandHandler<>), typeof(OutboxHandlerDecorator<>));
        builder.Services.Decorate(typeof(ICommandHandler<>), typeof(OpenTelemetryLoggingCommandHandlerDecorator<>));
        // Defining the source of request and its context
        builder.AddRunningContext(x => x.GetService<IHttpRunningContextProvider>());
            
        // Defining the error handling strategy
        builder.AddErrorHandler(c =>
        {
            c.Map<BrokenBusinessRuleException>((http, ex) => new BrokenBusinessRuleProblemDetails(ex));
            c.Map<CommandNotValidException>((http, ex) => new CommandNotValidProblemDetails(ex));
            c.Map<QueryNotValidException>((http, ex) => new QueryNotValidProblemDetails(ex));
            c.Map<DbUpdateConcurrencyException>((http, ex) => new ConcurrencyProblemDetails());
        });
            
        // ServiceBus register events
        /* builder.AddServiceBus();
        builder.AddServiceBusSubscriber<ServiceBusSubscriptionRegistrationInitializer>();
        builder.AddServiceBusPublisher<RemovalUserCommand>();
        builder.AddServiceBusWorker(); */
        builder.AddServiceBusPublisher<RemovalUserEvent>();
            
        return builder;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        // ServiceBus register event example
        /* var busSubscriber = builder.ApplicationServices.GetRequiredService<IEventSubscriber>();
        busSubscriber.Subscribe<CloseBookmakingEvent, CloseBookmakingEventHandler>(); */
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