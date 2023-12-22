using Honamic.IdentityPlus.Domain.Users;
using Honamic.IdentityPlus.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public abstract partial class IdentityPlusContext : DbContext
{
    public IdentityPlusContext(DbContextOptions options) : base(options) { }

    protected IdentityPlusContext() { }


    public virtual DbSet<User> Users { get; set; } = default!;
    public virtual DbSet<UserClaim> UserClaims { get; set; } = default!;
    public virtual DbSet<UserLogin> UserLogins { get; set; } = default!;
    public virtual DbSet<UserToken> UserTokens { get; set; } = default!;


    private StoreOptions? GetStoreOptions() => this.GetService<IDbContextOptions>()
                        .Extensions.OfType<CoreOptionsExtension>()
                        .FirstOrDefault()?.ApplicationServiceProvider
                        ?.GetService<IOptions<IdentityOptions>>()
                        ?.Value?.Stores;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.AddIdentityPlusModel(); 

    } 
}