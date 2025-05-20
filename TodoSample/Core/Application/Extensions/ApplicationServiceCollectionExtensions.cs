using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Honamic.Framework.Applications.Extensions;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Application.TodoItems.CommandHandlers;
using Honamic.Todo.Domain.Extensions;
using Honamic.Framework.Tools.IdGenerators;
using Honamic.Todo.Application.TodoItems.EventHandlers;
using Honamic.IdentityPlus.Domain.Users;
using Honamic.Framework.Applications.Results;

namespace Honamic.Todo.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultApplicationsServices();
        services.AddCommandHandlers();
        services.AddEventHandlers();
        services.AddDomainServices();
        services.AddSnowflakeIdGeneratorServices();
    }

    private static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddCommandHandler<DeleteTodoItemCommand, DeleteTodoItemCommandHandler>();
        services.AddCommandHandler<CreateTodoItemCommand, CreateTodoItemCommandHandler>();
        services.AddCommandHandler<
            CreateTodoItem2Command,
            CreateTodoItem2CommandHandler,
            Result<CreateTodoItem2ResultCommand>>();

        services.AddCommandHandler<MakeCompletedTodoItemCommand, MakeCompletedTodoItemCommandHandler>();
    }

    private static void AddEventHandlers(this IServiceCollection services)
    {
        services.AddEventHandler<UserCreatedEvent, UserCreatedEventHandler>();
        services.AddEventHandler<UserLoggedEvent, UserLoggedEventHandler>();
    }
}