using System;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.CQRS.Core.Events;
using Kitbag.Builder.CQRS.Core.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.CQRS.Core;

public static class Extensions
{
    public static IKitbagBuilder AddCQRS(this IKitbagBuilder builder, string sectionName = "CQRS")
    {
        if (!builder.TryRegisterKitBag(sectionName)) 
            return builder;

        builder.AddCommandHandlers();
        builder.AddInMemoryCommandDispatcher();

        builder.AddDomainEventHandlers();
        builder.AddInMemoryDomainEventDispatcher();

        builder.AddQueryHandlers();
        builder.AddInMemoryQueryDispatcher();

        return builder;
    }
        
    private static IKitbagBuilder AddCommandHandlers(this IKitbagBuilder builder, string sectionName = "Commands")
    {
        if (!builder.TryRegisterKitBag(sectionName))
            return builder;
            
        builder.Services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c =>
                {
                    c.AssignableTo(typeof(ICommandHandler<>));
                    c.Where((t) => !t.IsGenericType);
                })
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        return builder;
    }
        
    private static IKitbagBuilder AddInMemoryCommandDispatcher(this IKitbagBuilder builder)
    {
        builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        return builder;
    }

    private static IKitbagBuilder AddDomainEventHandlers(this IKitbagBuilder builder, string sectionName = "DomainEvents")
    {
        if (!builder.TryRegisterKitBag(sectionName))
            return builder;
            
        builder.Services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        return builder;
    }
    private static IKitbagBuilder AddInMemoryDomainEventDispatcher(this IKitbagBuilder builder)
    {
        builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        return builder;
    }

    private static IKitbagBuilder AddQueryHandlers(this IKitbagBuilder builder)
    {
        builder.Services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .WithTransientLifetime());

        return builder;
    }

    private static IKitbagBuilder AddInMemoryQueryDispatcher(this IKitbagBuilder builder)
    {
        builder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        return builder;
    }
}