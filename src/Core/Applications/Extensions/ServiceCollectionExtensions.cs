using Honamic.Framework.Applications.CommandHandlerDecorators;
using Honamic.Framework.Applications.QueryHandlerDecorators;
using Honamic.Framework.Commands;
using Honamic.Framework.Commands.Extensions;
using Honamic.Framework.Domain.Extensions;
using Honamic.Framework.Events;
using Honamic.Framework.Events.Extensions;
using Honamic.Framework.Queries;
using Honamic.Framework.Queries.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Framework.Applications.Extensions;

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

    public static void AddQueryHandler<TQueryFilter, TQueryResult, TQueryHandler>(this IServiceCollection services)
        where TQueryFilter : IQueryFilter
        //where TQueryResult : IQueryResult
        where TQueryHandler : class, IQueryHandler<TQueryFilter, TQueryResult>
    {
        services.AddTransient<IQueryHandler<TQueryFilter, TQueryResult>, TQueryHandler>();
        services.Decorate<IQueryHandler<TQueryFilter, TQueryResult>, AuthorizeQueryHandlerDecorator<TQueryFilter, TQueryResult>>();
        services.Decorate<IQueryHandler<TQueryFilter, TQueryResult>, ExceptionQueryHandlerDecorator<TQueryFilter, TQueryResult>>();

    }
}