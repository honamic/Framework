using Honamic.Framework.Application.CommandHandlerDecorators;
using Honamic.Framework.Application.QueryHandlerDecorators;
using Honamic.Framework.Commands;
using Honamic.Framework.Events;
using Honamic.Framework.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Honamic.Framework.Application.Extensions;

public static class HandlerRegistrationExtensions
{
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

        RegisterCommandHandlers(services, assemblies);
        RegisterQueryHandlers(services, assemblies);
        RegisterEventHandlers(services, assemblies);

        return services;
    }


    public static void AddCommandHandler<TCommandHandler>(this IServiceCollection services)
        where TCommandHandler : class
    {
        RegisterCommandHandler(services, typeof(TCommandHandler));
    }

    public static void AddEventHandler<TEventHandler>(this IServiceCollection services)
        where TEventHandler : class
    {
        RegisterEventHandler(services, typeof(TEventHandler));
    }

    public static void AddQueryHandler<TQueryHandler>(this IServiceCollection services)
        where TQueryHandler : class
    {
        RegisterQueryHandler(services, typeof(TQueryHandler));
    }


    private static void RegisterCommandHandler(IServiceCollection services, Type handlerType)
    {
        var interfaces = handlerType.GetInterfaces();

        var commandInterface = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
        if (commandInterface != null)
        {
            var commandType = commandInterface.GetGenericArguments()[0];
            var interfaceType = typeof(ICommandHandler<>).MakeGenericType(commandType);
            services.AddTransient(interfaceType, handlerType);
            services.Decorate(interfaceType, typeof(AuthorizeCommandHandlerDecorator<>).MakeGenericType(commandType));
            services.Decorate(interfaceType, typeof(TransactionalCommandHandlerDecorator<>).MakeGenericType(commandType));
            return;
        }

        var commandWithResponseInterface = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
        if (commandWithResponseInterface != null)
        {
            var genericArgs = commandWithResponseInterface.GetGenericArguments();
            var commandType = genericArgs[0];
            var responseType = genericArgs[1];
            var interfaceType = typeof(ICommandHandler<,>).MakeGenericType(commandType, responseType);
            services.AddTransient(interfaceType, handlerType);
            services.Decorate(interfaceType, typeof(AuthorizeCommandHandlerDecorator<,>).MakeGenericType(commandType, responseType));
            services.Decorate(interfaceType, typeof(TransactionalCommandHandlerDecorator<,>).MakeGenericType(commandType, responseType));
            services.Decorate(interfaceType, typeof(ExceptionCommandHandlerDecorator<,>).MakeGenericType(commandType, responseType));
            return;
        }

        throw new InvalidOperationException($"The handler {handlerType.Name} does not implement ICommandHandler<T> or ICommandHandler<T, TResponse>.");
    }

    private static void RegisterEventHandler(IServiceCollection services, Type handlerType)
    {
        var interfaces = handlerType.GetInterfaces();

        var eventInterface = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
        if (eventInterface == null)
        {
            throw new InvalidOperationException($"The handler {handlerType.Name} does not implement IEventHandler<T>.");
        }

        var eventType = eventInterface.GetGenericArguments()[0];
        var interfaceType = typeof(IEventHandler<>).MakeGenericType(eventType);
        services.AddTransient(interfaceType, handlerType);
    }

    private static void RegisterQueryHandler(IServiceCollection services, Type handlerType)
    {
        var interfaces = handlerType.GetInterfaces();

        var queryInterface = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
        if (queryInterface == null)
        {
            throw new InvalidOperationException($"The handler {handlerType.Name} does not implement IQueryHandler<T, TResponse>.");
        }

        var genericArgs = queryInterface.GetGenericArguments();
        var queryType = genericArgs[0];
        var responseType = genericArgs[1];
        var interfaceType = typeof(IQueryHandler<,>).MakeGenericType(queryType, responseType);
        services.AddTransient(interfaceType, handlerType);
        services.Decorate(interfaceType, typeof(AuthorizeQueryHandlerDecorator<,>).MakeGenericType(queryType, responseType));
        services.Decorate(interfaceType, typeof(ExceptionQueryHandlerDecorator<,>).MakeGenericType(queryType, responseType));
    }

    private static void RegisterCommandHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        var commandHandlerTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && 
                (t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)) ||
                 t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))));

        foreach (var handlerType in commandHandlerTypes)
        {
            RegisterCommandHandler(services, handlerType);
        }
    }

    private static void RegisterQueryHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        var queryHandlerTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

        foreach (var handlerType in queryHandlerTypes)
        {
            RegisterQueryHandler(services, handlerType);
        }
    }

    private static void RegisterEventHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        var eventHandlerTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

        foreach (var handlerType in eventHandlerTypes)
        {
            RegisterEventHandler(services, handlerType);
        }
    }
}