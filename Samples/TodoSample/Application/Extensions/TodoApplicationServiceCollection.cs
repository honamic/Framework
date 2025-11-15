using Honamic.Framework.Application.Authorizes;
using Honamic.Framework.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TodoSample.Application.Todos.CommandHandlers;
using TodoSample.Application.Todos.QueryHandlers;
using TodoSample.Domain.Extensions;

namespace TodoSample.Application.Extensions;

public static class TodoApplicationServiceCollection
{
    public static IServiceCollection AddTodoApplicationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        DynamicPermissionRegistry.RegisterAssemblies(typeof(TodoConstants).Assembly, typeof(TodoApplicationServiceCollection).Assembly);

        services.AddTodoDomainServices();

        var AutoScanAndRegisterHandlers = true;

        if (AutoScanAndRegisterHandlers)
        {
            services.AddHandlersFromAssemblies();
        }
        else
        {
            services.AddCommandHandlers();
            services.AddQueryHandlers();
            services.AddEventHandlers();
        }

        return services;
    }

    private static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddCommandHandler<CreateTodoCommandHandler>();
    }

    private static void AddQueryHandlers(this IServiceCollection services)
    {
        services.AddQueryHandler<GetTodoQueryHandler>();
        services.AddQueryHandler<GetAllTodosQueryHandler>();
    }

    private static void AddEventHandlers(this IServiceCollection services)
    {
        //services.AddEventHandler<UserCreatedEventHandler>();
    }
}