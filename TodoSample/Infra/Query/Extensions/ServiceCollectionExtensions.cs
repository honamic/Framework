using Honamic.Framework.Persistence.EntityFramework.Extensions;
using Honamic.Todo.Query.Domain.TodoItems;
using Honamic.Todo.Query.EntityFramework.TodoItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Honamic.Todo.Query.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddQueryEntityFrameworkServices(this IServiceCollection services, IConfiguration configuration)
    {
        var SqlServerConnection = configuration.GetConnectionString("SqlServerConnectionString");
        DebuggerConnectionStringLog(SqlServerConnection);
        services.AddDbContext<TodoQueryDbContext>(options =>
        {
            options.UseSqlServer(SqlServerConnection);
            options.AddPersianYeKeCommandInterceptor();
            DebuggerConsoleLog(options);
        });
        services.AddTransient<ITodoItemQueryRepository, TodoItemQueryRepository>();
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