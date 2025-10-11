using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoSample.QueryModels.Todos;

namespace TodoSample.QueryModels.EntityFramework.Todos.Configurations;

public class TodoQueryModelConfiguration : IEntityTypeConfiguration<TodoQueryModel>
{
    public void Configure(EntityTypeBuilder<TodoQueryModel> builder)
    {
        builder.ToTable("Todos");

        builder.HasKey(t => t.Id);

    }
}