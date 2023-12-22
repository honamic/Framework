using Honamic.IdentityPlus.Domain.Roles;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Honamic.IdentityPlus.Persistence.Users;
internal class RoleEntityConfigurations : IEntityTypeConfiguration<Role>
{
    private readonly string tableName;
    private readonly string? schema;
    public RoleEntityConfigurations(string tableName, string? schema)
    {
        this.tableName = tableName;
        this.schema = schema;
    }

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.NormalizedName)
            .HasDatabaseName("RoleNameIndex")
            .IsUnique();

        builder.ToTable(tableName, schema);

        builder.Property(r => r.ConcurrencyStamp)
            .IsConcurrencyToken();

        builder.Property(u => u.Name).HasMaxLength(256);
        builder.Property(u => u.NormalizedName).HasMaxLength(256);

        builder.HasMany<UserRole>()
            .WithOne()
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        builder.HasMany<RoleClaim>()
            .WithOne()
            .HasForeignKey(rc => rc.RoleId).IsRequired();

    }
}