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

        RegisterCommandHandlersWithoutResponse(services, assemblies);
        RegisterCommandHandlersWithResponse(services, assemblies);
        RegisterQueryHandlers(services, assemblies);
        RegisterEventHandlers(services, assemblies);

        return services;
    }

    private static void RegisterCommandHandlersWithoutResponse(IServiceCollection services, Assembly[] assemblies)
    {
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
    }

    private static void RegisterCommandHandlersWithResponse(IServiceCollection services, Assembly[] assemblies)
    {
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
    }

    private static void RegisterQueryHandlers(IServiceCollection services, Assembly[] assemblies)
    {
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
    }

    private static void RegisterEventHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        var eventHandlerTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

        foreach (var handlerType in eventHandlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
            services.AddTransient(interfaceType, handlerType);
        }
    }
}