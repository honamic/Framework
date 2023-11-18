using Honamic.Todo.Domain;
using Honamic.Todo.Domain.TodoItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honamic.Todo.Persistence.EntityFramework.TodoItems;

internal class TodoItemEntityConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.OwnsMany(x => x.Logs, map =>
        {
            map.HasKey(o => o.Id);
            map.ToTable(nameof(TodoItemLog), Constants.Schema);
            map.WithOwner().HasForeignKey(o => o.TodoItemRef);
            map.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        });

        builder.ToTable(nameof(TodoItem), Constants.Schema);
    }
}