using Honamic.Framework.EntityFramework.Query;
using Honamic.Todo.Query.Domain.TodoItems.Models;
using Honamic.Todo.Query.EntityFramework.TodoItems.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Honamic.Todo.Query.EntityFramework;

public class TodoQueryDbContext : QueryDbContext‌Base
{
    public DbSet<TodoItemQuery> TodoItems { get; set; }
    public TodoQueryDbContext(DbContextOptions<TodoQueryDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoItemQueryEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TodoItemLogQueryEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}