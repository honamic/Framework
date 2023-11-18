using Honamic.Todo.Query.Domain.TodoItems.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honamic.Todo.Query.EntityFramework.TodoItems.EntityConfigurations;

public class TodoItemLogQueryEntityConfiguration : IEntityTypeConfiguration<TodoItemLogQuery>
{
    public void Configure(EntityTypeBuilder<TodoItemLogQuery> builder)
    {
        builder.HasKey(o => o.Id);
        builder.ToTable("TodoItemLog", "todo");
        builder.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.TodoItemRef);
    }
}