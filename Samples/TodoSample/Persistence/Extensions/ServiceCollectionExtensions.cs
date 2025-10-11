using Honamic.Framework.Persistence.EntityFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TodoSample.Domain.Todos;
using Microsoft.EntityFrameworkCore;
using TodoSample.Persistence.Todos;
using Honamic.Framework.EntityFramework.Persistence.Extensions;

namespace TodoSample.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddTodoPersistenceServices(this IServiceCollection services, string sqlServerConnection)
    {
        services.AddDbContext<TodoSampleDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(sqlServerConnection);
            options.AddPersianYeKeCommandInterceptor();
            options.AddAuditFieldsSaveChangesInterceptor(serviceProvider,
                Honamic.Framework.EntityFramework.Interceptors.AuditFields.AuditType.UserNameAndId);
            options.AddAggregateRootVersionInterceptor(serviceProvider);
            options.AddMarkAsDeletedInterceptors();
        });

        services.AddUnitOfWorkByEntityFramework<TodoSampleDbContext>();

        services.AddTransient<ITodoRepository, TodoRepository>();
    }
}