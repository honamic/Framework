using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honamic.IdentityPlus.Persistence.Users;
internal class UserConfigurations : IEntityTypeConfiguration<User>
{
    private readonly string tableName;
    private readonly string? schema;
    private bool _encryptPersonalData;
    private PersonalDataConverter? _personalDataConverter;
    public UserConfigurations(string tableName, string? schema, bool encryptPersonalData, PersonalDataConverter personalDataConverter = null)
    {
        this.tableName = tableName;
        this.schema = schema;
        _encryptPersonalData = encryptPersonalData;
        _personalDataConverter = personalDataConverter;
    }

    public void Configure(EntityTypeBuilder<User> b)
    {
        b.HasKey(u => u.Id);
        b.HasIndex(u => u.NormalizedUserName)
            .HasDatabaseName("UserNameIndex")
            .IsUnique();

        b.HasIndex(u => u.NormalizedEmail)
            .HasDatabaseName("EmailIndex");

        b.ToTable(tableName, schema);

        b.Property(u => u.ConcurrencyStamp)
            .IsConcurrencyToken();

        b.Property(u => u.UserName).HasMaxLength(256);
        b.Property(u => u.NormalizedUserName).HasMaxLength(256);
        b.Property(u => u.Email).HasMaxLength(256);
        b.Property(u => u.NormalizedEmail).HasMaxLength(256);
        b.Property(u => u.PhoneNumber).HasMaxLength(256);


        if (_encryptPersonalData)
        {
            var personalDataProps = typeof(User).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));

            foreach (var p in personalDataProps)
            {
                if (p.PropertyType != typeof(string))
                {
                    throw new InvalidOperationException("[ProtectedPersonalData] only works strings by default.");
                }
                b.Property(typeof(string), p.Name)
                    .HasConversion(_personalDataConverter);
            }
        }

        b.HasMany<UserClaim>()
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

        b.HasMany<UserLogin>()
            .WithOne()
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        b.HasMany<UserToken>()
            .WithOne()
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        b.HasMany<UserRole>()
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
        .IsRequired();

    }
}
