using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Honamic.Framework.Facade.FastCrud;

public abstract class BaseEntityFacade : BaseFacade
{
    protected readonly FastCrudDbContext CrudDbContext;

    public BaseEntityFacade(ILogger logger, FastCrudDbContext dbContext)
        : base(logger)
    {
        CrudDbContext = dbContext;
    }
}

public abstract class BaseEntityFacade<TEntity> : BaseEntityFacade where TEntity : class
{
    protected readonly DbSet<TEntity> Dbset;

    public BaseEntityFacade(ILogger logger, FastCrudDbContext dbContext)
        : base(logger, dbContext)
    {
        Dbset = dbContext.Set<TEntity>();
    }
}