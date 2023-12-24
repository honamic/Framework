using Honamic.Todo.Persistence.EntityFramework.TodoItems;
using Microsoft.EntityFrameworkCore;
using Honamic.IdentityPlus.Persistence.Extensions;
namespace Honamic.Todo.Persistence.EntityFramework;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoItemEntityConfiguration());

        modelBuilder.AddIdentityPlusModel();

        base.OnModelCreating(modelBuilder);
    }
}
