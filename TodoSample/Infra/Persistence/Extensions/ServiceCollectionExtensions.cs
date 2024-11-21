using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Honamic.Framework.Persistence.EntityFramework.Extensions;
using Honamic.Todo.Domain.TodoItems;
using Honamic.Todo.Domain;
using Honamic.Todo.Persistence.EntityFramework.TodoItems;
using Honamic.IdentityPlus.Persistence.Extensions;


namespace Honamic.Todo.Persistence.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceEntityFrameworkServices(this IServiceCollection services, IConfiguration configuration)
    {
        var SqlServerConnection = configuration.GetConnectionString("SqlServerConnectionString");
        DebuggerConnectionStringLog(SqlServerConnection);
        services.AddDbContext<TodoDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(SqlServerConnection, x => x.MigrationsHistoryTable("__EFMigrationsHistory", Constants.Schema));
            options.AddPersianYeKeCommandInterceptor();
            options.AddAggregateRootVersionInterceptor(serviceProvider);
            options.AddMarkAsDeletedInterceptors();
            DebuggerConsoleLog(options);
        });

        services.AddTransient<ITodoItemRepository, TodoItemRepository>();
        services.AddUnitOfWorkByEntityFramework<TodoDbContext>();

        services.AddIdentityPlusPersistence();
    }

    private static void DebuggerConnectionStringLog(string? SqlServerConnection)
    {
#if DEBUG
        Console.WriteLine($"EF connection string:`{SqlServerConnection}`");
#endif

    }

    private static void DebuggerConsoleLog(DbContextOptionsBuilder options)
    {
#if DEBUG
        options.LogTo(c => Console.WriteLine(c));
#endif
    }
}