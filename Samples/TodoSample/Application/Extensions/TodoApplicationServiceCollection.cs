using Honamic.Framework.Application.Authorizes;
using Honamic.Framework.Application.Extensions;
using Honamic.Framework.Application.Results;
using Honamic.Framework.Queries;
using Microsoft.Extensions.DependencyInjection;
using TodoSample.Application.Contracts.Todos.Commands;
using TodoSample.Application.Contracts.Todos.Queries;
using TodoSample.Application.Todos.CommandHandlers;
using TodoSample.Application.Todos.QueryHandlers;
using TodoSample.Domain.Extensions;

namespace TodoSample.Application.Extensions;

public static class TodoApplicationServiceCollection
{
    public static IServiceCollection AddTodoApplicationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        DynamicPermissionRegistry.RegisterAssemblies(typeof(TodoApplicationServiceCollection).Assembly);
        DynamicPermissionRegistry.RegisterAssemblies(typeof(TodoConstants).Assembly);

        services.AddTodoDomainServices();
        services.AddHandlersFromAssemblies();

        //services.AddCommandHandlers();
        //services.AddQueryHandlers();
        //services.AddEventHandlers();

        return services;
    }

    private static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddCommandHandler<CreateTodoCommand, CreateTodoCommandHandler, Result<CreateTodoCommandResult>>();
    }

    private static void AddQueryHandlers(this IServiceCollection services)
    {
        services.AddQueryHandler<GetTodoQuery, Result<GetTodoQueryResult?>, GetTodoQueryHandler>();
        services.AddQueryHandler<GetAllTodosQuery, Result<PagedQueryResult<GetAllTodosQueryResult>>, GetAllTodosQueryHandler>();
    }

    private static void AddEventHandlers(this IServiceCollection services)
    {
        //services.AddEventHandler<UserCreatedEvent, UserCreatedEventHandler>();
    }
}