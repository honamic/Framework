using Microsoft.EntityFrameworkCore; 
using TodoSample.Persistence.Todos.Configurations;

namespace TodoSample.Persistence;
public class TodoSampleDbContext : DbContext
{
    public TodoSampleDbContext(DbContextOptions<TodoSampleDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}