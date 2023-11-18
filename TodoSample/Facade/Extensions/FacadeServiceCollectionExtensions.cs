using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Honamic.Framework.Facade.Extensions;
using Honamic.Todo.Application.Extensions;
using Honamic.Todo.Persistence.EntityFramework.Extensions;
using Honamic.Todo.Facade.TodoItems;
using Honamic.Todo.Query.EntityFramework.Extensions;

namespace Honamic.Todo.Facade.Extensions;

public static class FacadeServiceCollectionExtensions
{
    public static void AddFacades(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices(configuration);
        services.AddDefaultFrameworkFacadeServices(configuration);

        services.AddPersistenceEntityFrameworkServices(configuration);
        services.AddQueryEntityFrameworkServices(configuration);

        services.AddFacades();
    }

    private static void AddFacades(this IServiceCollection services)
    {
        services.AddFacadeScoped<ITodoItemFacade, TodoItemFacade>();
        services.AddFacadeScoped<ITodoItemQueryFacade, TodoItemQueryFacade>();
    }
}