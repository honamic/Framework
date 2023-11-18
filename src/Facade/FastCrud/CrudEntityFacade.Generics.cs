using Honamic.Framework.Domain;
using Honamic.Framework.Facade.FastCrud.Dtos;
using Honamic.Framework.Queries;
using Microsoft.Extensions.Logging;

namespace Honamic.Framework.Facade.FastCrud;

public abstract class CrudEntityFacade<TEntity, TEntityDto> :
    CrudEntityFacade<TEntity, TEntityDto, long>,
    ICrudEntityFacade<TEntity, TEntityDto>
    where TEntity : Entity<long>
    where TEntityDto : EntityDto<TEntityDto, TEntity, long>
{
    protected CrudEntityFacade(ILogger logger, FastCrudDbContext dbContext, IServiceProvider serviceProvider) :
        base(logger, dbContext, serviceProvider)
    {

    }
}

public abstract class CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey> :
     CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntityDto>,
     ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey>

     where TEntity : Entity<TPrimaryKey>
     where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
{
    protected CrudEntityFacade(ILogger logger, FastCrudDbContext dbContext, IServiceProvider httpContextAccessor) :
        base(logger, dbContext, httpContextAccessor)
    {

    }
}

public abstract class CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto> :
   CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, PagedQueryFilter>,
   ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto>

   where TEntity : Entity<TPrimaryKey>
   where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
     where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
{
    protected CrudEntityFacade(ILogger logger, FastCrudDbContext dbContext, IServiceProvider httpContextAccessor) :
        base(logger, dbContext, httpContextAccessor)
    {

    }
}

public abstract class CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput> :
       CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TEntityDto>,
       ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput>
       where TEntity : Entity<TPrimaryKey>
       where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
     where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
       where TGetAllInput : PagedQueryFilter

{
    protected CrudEntityFacade(ILogger logger, FastCrudDbContext dbContext, IServiceProvider httpContextAccessor) :
        base(logger, dbContext, httpContextAccessor)
    {

    }
}


public abstract class CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput> :
 CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TCreateInput>,
 ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput>
 where TEntity : Entity<TPrimaryKey>
 where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
     where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
 where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
 where TGetAllInput : PagedQueryFilter
{
    protected CrudEntityFacade(ILogger logger, FastCrudDbContext dbContext, IServiceProvider httpContextAccessor) :
        base(logger, dbContext, httpContextAccessor)
    {

    }
}

public abstract class CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput> :
 CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TEntityDto>,
 ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput>
 where TEntity : Entity<TPrimaryKey>
 where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
  where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
 where TUpdateInput : EntityDto<TUpdateInput, TEntity, TPrimaryKey>
 where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
 where TGetAllInput : PagedQueryFilter
{
    protected CrudEntityFacade(ILogger logger, FastCrudDbContext dbContext, IServiceProvider httpContextAccessor) :
        base(logger, dbContext, httpContextAccessor)
    {

    }
}

public abstract class CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult> :
    CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, PagedQueryFilter>,
    ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, PagedQueryFilter>
    where TEntity : Entity<TPrimaryKey>
    where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
     where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
    where TUpdateInput : EntityDto<TUpdateInput, TEntity, TPrimaryKey>
    where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
    where TGetAllInput : PagedQueryFilter
    where TSelectResult : EntityDto<TSelectResult, TEntity, TPrimaryKey>

{
    protected CrudEntityFacade(ILogger logger, FastCrudDbContext dbContext, IServiceProvider httpContextAccessor) :
        base(logger, dbContext, httpContextAccessor)
    {

    }
}