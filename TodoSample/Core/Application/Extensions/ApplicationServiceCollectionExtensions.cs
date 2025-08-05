using Honamic.Framework.Applications.Extensions;
using Honamic.Framework.Applications.Results;
using Honamic.Framework.Queries;
using Honamic.Framework.Tools.IdGenerators;
using Honamic.IdentityPlus.Application.Users.QueryHandlers;
using Honamic.Todo.Application.TodoItems.CommandHandlers;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Domain.Extensions;
using Honamic.Todo.Query.Domain.TodoItems.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Todo.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultApplicationsServices();
        services.AddCommandHandlers();
        services.AddQueryHandlers();
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

    }

    private static void AddQueryHandlers(this IServiceCollection services)
    {
        services.AddQueryHandler<GetAllTodoItemsQueryFilter, Result<PagedQueryResult<GetAllTodoItemsQueryResult>>, GetAllTodoItemsQueryHandler>();
    }
}