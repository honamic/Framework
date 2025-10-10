using Microsoft.EntityFrameworkCore;

namespace Honamic.Framework.EntityFramework.QueryModels;
public abstract class QueryDbContext‌Base : DbContext
{
    public QueryDbContext‌Base()
    {

    }

    public QueryDbContext‌Base(DbContextOptions options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }


    #region SaveChanges
    public override int SaveChanges()
    {
        throw NotSupportException();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw NotSupportException();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        throw NotSupportException();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw NotSupportException();
    }

    private static Exception NotSupportException()
    {
        return new Exception($"{nameof(QueryDbContext‌Base)} does not support saving changes");
    }
    #endregion
}
