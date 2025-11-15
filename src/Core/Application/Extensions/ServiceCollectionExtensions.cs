using Honamic.Framework.Application.CommandHandlerDecorators;
using Honamic.Framework.Application.QueryHandlerDecorators;
using Honamic.Framework.Commands;
using Honamic.Framework.Commands.Extensions;
using Honamic.Framework.Domain.Extensions;
using Honamic.Framework.Events;
using Honamic.Framework.Events.Extensions;
using Honamic.Framework.Queries;
using Honamic.Framework.Queries.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Honamic.Framework.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultApplicationsServices(this IServiceCollection services)
    {
        services.AddDefaultDomainServices();
        services.AddDefaultEventsServices();
        services.AddDefaultCommandsServices();
        services.AddDefaultQueriesServices();

        return services;
    }

    public static void AddCommandHandler<TCommand, TCommandHandler>(this IServiceCollection services)
     where TCommand : ICommand
     where TCommandHandler : class, ICommandHandler<TCommand>
    {
        services.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
        services.Decorate<ICommandHandler<TCommand>, AuthorizeCommandHandlerDecorator<TCommand>>();
        services.Decorate<ICommandHandler<TCommand>, TransactionalCommandHandlerDecorator<TCommand>>();
        // Note: No ResultOriented decorator for non-response commands
    }

    public static void AddCommandHandler<TCommand, TCommandHandler, TResponse>(this IServiceCollection services)
     where TCommand : ICommand<TResponse>
     where TCommandHandler : class, ICommandHandler<TCommand, TResponse>
    {
        services.AddTransient<ICommandHandler<TCommand, TResponse>, TCommandHandler>();
        services.Decorate<ICommandHandler<TCommand, TResponse>, AuthorizeCommandHandlerDecorator<TCommand, TResponse>>();
        services.Decorate<ICommandHandler<TCommand, TResponse>, TransactionalCommandHandlerDecorator<TCommand, TResponse>>();
        services.Decorate<ICommandHandler<TCommand, TResponse>, ExceptionCommandHandlerDecorator<TCommand, TResponse>>();
    }

    public static void AddEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
    where TEvent : IEvent
    where TEventHandler : class, IEventHandler<TEvent>
    {
        services.AddTransient<IEventHandler<TEvent>, TEventHandler>();
    }

    public static void AddQueryHandler<TQuery, TResponse, TQueryHandler>(this IServiceCollection services)
    where TQuery : class, IQuery<TResponse>
    where TQueryHandler : class, IQueryHandler<TQuery, TResponse>
    {
        services.AddTransient<IQueryHandler<TQuery, TResponse>, TQueryHandler>();
        services.Decorate<IQueryHandler<TQuery, TResponse>, AuthorizeQueryHandlerDecorator<TQuery, TResponse>>();
        services.Decorate<IQueryHandler<TQuery, TResponse>, ExceptionQueryHandlerDecorator<TQuery, TResponse>>();
    }

    /// <summary>
    /// Automatically registers command handlers, query handlers, and event handlers from the specified assemblies.
    /// This method scans the assemblies for classes implementing <see cref="ICommandHandler{TCommand}"/>,
    /// <see cref="ICommandHandler{TCommand, TResponse}"/>, <see cref="IQueryHandler{TQuery, TResponse}"/>,
    /// and <see cref="IEventHandler{TEvent}"/>, registers them as transient services, and applies the appropriate decorators.
    /// If no assemblies are provided, it defaults to scanning the calling assembly.
    /// </summary>
    /// <param name="services">The service collection to add the handlers to.</param>
    /// <param name="assemblies">The assemblies to scan for handlers. If empty, uses the calling assembly.</param>
    /// <returns>The service collection with the handlers registered.</returns>
    public static IServiceCollection AddHandlersFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
        {
            assemblies = new[] { Assembly.GetCallingAssembly() };
        }

        // Scan for command handlers without response
        var commandHandlerTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

        foreach (var handlerType in commandHandlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
            var commandType = interfaceType.GetGenericArguments()[0];
            services.AddTransient(interfaceType, handlerType);
            services.Decorate(interfaceType, typeof(AuthorizeCommandHandlerDecorator<>).MakeGenericType(commandType));
            services.Decorate(interfaceType, typeof(TransactionalCommandHandlerDecorator<>).MakeGenericType(commandType));
        }

        // Scan for command handlers with response
        var commandHandlerWithResponseTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));

        foreach (var handlerType in commandHandlerWithResponseTypes)
        {
            var interfaceType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
            var genericArgs = interfaceType.GetGenericArguments();
            var commandType = genericArgs[0];
            var responseType = genericArgs[1];
            services.AddTransient(interfaceType, handlerType);
            services.Decorate(interfaceType, typeof(AuthorizeCommandHandlerDecorator<,>).MakeGenericType(commandType, responseType));
            services.Decorate(interfaceType, typeof(TransactionalCommandHandlerDecorator<,>).MakeGenericType(commandType, responseType));
            services.Decorate(interfaceType, typeof(ExceptionCommandHandlerDecorator<,>).MakeGenericType(commandType, responseType));
        }

        // Scan for query handlers
        var queryHandlerTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

        foreach (var handlerType in queryHandlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
            var genericArgs = interfaceType.GetGenericArguments();
            var queryType = genericArgs[0];
            var responseType = genericArgs[1];
            services.AddTransient(interfaceType, handlerType);
            services.Decorate(interfaceType, typeof(AuthorizeQueryHandlerDecorator<,>).MakeGenericType(queryType, responseType));
            services.Decorate(interfaceType, typeof(ExceptionQueryHandlerDecorator<,>).MakeGenericType(queryType, responseType));
        }

        // Scan for event handlers
        var eventHandlerTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

        foreach (var handlerType in eventHandlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
            services.AddTransient(interfaceType, handlerType);
        }

        return services;
    }
}