using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Honamic.Framework.Applications.Extensions;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Application.TodoItems.CommandHandlers;

namespace Honamic.Notifo.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultApplicationsServices();
        services.AddCommandHandlers();
    }

    private static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddCommandHandler<DeleteTodoItemCommand, DeleteTodoItemCommandHandler>();
        services.AddCommandHandler<CreateTodoItemCommand, CreateTodoItemCommandHandler>();
    }
}