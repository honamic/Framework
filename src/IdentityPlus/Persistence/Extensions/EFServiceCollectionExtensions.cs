using Honamic.IdentityPlus.Domain.Roles;
using Honamic.IdentityPlus.Domain.Users;
using Honamic.IdentityPlus.Persistence.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Honamic.IdentityPlus.Persistence.Extensions;

public static class EFServiceCollectionExtensions
{
    public static void AddIdentityPlusModel(this ModelBuilder modelBuilder
    , string? schema = null)
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        //IOptions<IdentityOption>.Stores?.MaxLengthForKeys
        // if (maxKeyLength == 0) {maxKeyLength = 128;}
        var maxLengthForKeys = 128;
        var encryptPersonalData = false;
        PersonalDataConverter? _personalDataConverter = null;

        modelBuilder.ApplyConfiguration(new UserConfigurations("Users", schema, encryptPersonalData, _personalDataConverter));

        modelBuilder.Entity<UserClaim>(b =>
        {
            b.HasKey(uc => uc.Id);
            b.ToTable("UserClaims", schema);
        });

        modelBuilder.Entity<UserLogin>(b =>
        {
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
            b.Property(l => l.LoginProvider).HasMaxLength(maxLengthForKeys);
            b.Property(l => l.ProviderKey).HasMaxLength(maxLengthForKeys);
            b.ToTable("UserLogins");
        });

        modelBuilder.Entity<UserToken>(b =>
        {
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            b.Property(t => t.LoginProvider).HasMaxLength(maxLengthForKeys);
            b.Property(t => t.Name).HasMaxLength(maxLengthForKeys);

            if (encryptPersonalData)
            {
                var tokenProps = typeof(UserToken).GetProperties().Where(
                                prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                foreach (var p in tokenProps)
                {
                    if (p.PropertyType != typeof(string))
                    {
                        throw new InvalidOperationException("[ProtectedPersonalData] only works strings by default.");
                    }
                    b.Property(typeof(string), p.Name)
                    .HasConversion(_personalDataConverter);
                }
            }

            b.ToTable("UserTokens");
        });


        modelBuilder.ApplyConfiguration(new RoleEntityConfigurations("Roles", schema));

        modelBuilder.Entity<RoleClaim>(b =>
        {
            b.HasKey(rc => rc.Id);
            b.ToTable("RoleClaims");
        });

        modelBuilder.Entity<UserRole>(b =>
        {
            b.HasKey(r => new { r.UserId, r.RoleId });
            b.ToTable("UserRoles");
        });
    }
}
