using Honamic.IdentityPlus.Domain;
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
    public UserConfigurations(string tableName, string? schema, bool encryptPersonalData, PersonalDataConverter? personalDataConverter = null)
    {
        this.tableName = tableName;
        this.schema = schema;
        _encryptPersonalData = encryptPersonalData;
        _personalDataConverter = personalDataConverter;
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.HasIndex(u => u.NormalizedUserName)
            .HasDatabaseName("UserNameIndex")
            .IsUnique();

        builder.HasIndex(u => u.NormalizedEmail)
            .HasDatabaseName("EmailIndex");

        builder.ToTable(tableName, schema);

        builder.Property(u => u.ConcurrencyStamp)
            .IsConcurrencyToken();

        builder.Property(u => u.UserName).HasMaxLength(256);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
        builder.Property(u => u.Email).HasMaxLength(256);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
        builder.Property(u => u.PhoneNumber).HasMaxLength(256);


        if (_encryptPersonalData)
        {
            var personalDataProps = typeof(User).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)) ||
                                 Attribute.IsDefined(prop, typeof(IdentityPlusProtectedPersonalDataAttribute)));

            foreach (var p in personalDataProps)
            {
                if (p.PropertyType != typeof(string))
                {
                    throw new InvalidOperationException("[ProtectedPersonalData] only works strings by default.");
                }
                builder.Property(typeof(string), p.Name)
                    .HasConversion(_personalDataConverter);
            }
        }

        builder.HasMany<UserClaim>()
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

        builder.HasMany<UserLogin>()
            .WithOne()
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        builder.HasMany<UserToken>()
            .WithOne()
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        builder.HasMany<UserRole>()
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
        .IsRequired();

    }
}
