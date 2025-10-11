using Honamic.Framework.EntityFramework.QueryModels.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TodoSample.QueryModels.EntityFramework.Todos;
using Microsoft.EntityFrameworkCore;
using Honamic.Framework.Persistence.EntityFramework.Extensions;
using TodoSample.QueryModels.Todos;

namespace TodoSample.QueryModels.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddTodoQueryModelsServices(this IServiceCollection services, string sqlServerConnection)
    {
        services.AddDbContext<TodoQueryModelDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(sqlServerConnection);
            options.AddPersianYeKeCommandInterceptor();
        });

        services.AddDefaultQueryModelDbContext<TodoQueryModelDbContext>();

        services.AddTransient<ITodoQueryModelRepository, TodoQueryModelRepository>();
    }
}