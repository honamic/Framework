using Honamic.Todo.Query.Domain.TodoItems.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honamic.Todo.Query.EntityFramework.TodoItems.EntityConfigurations;

internal class TodoItemQueryEntityConfiguration : IEntityTypeConfiguration<TodoItemQuery>
{
    public void Configure(EntityTypeBuilder<TodoItemQuery> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.HasMany(x => x.Logs)
            .WithOne()
            .HasForeignKey(x => x.TodoItemRef);

        builder.ToTable("TodoItem", "todo");

    }
}