using Honamic.Framework.EntityFramework.QueryModels;
using Microsoft.EntityFrameworkCore;
using TodoSample.QueryModels.EntityFramework.Todos.Configurations;

namespace TodoSample.QueryModels.EntityFramework;
public class TodoQueryModelDbContext : QueryDbContextBase
{
    public TodoQueryModelDbContext(DbContextOptions<TodoQueryModelDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TodoQueryModelConfiguration());
       
        base.OnModelCreating(modelBuilder);
    }
}