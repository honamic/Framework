using Honamic.Framework.Domain;
using Honamic.Framework.Facade.FastCrud.Dtos;
using Honamic.Framework.Queries;
namespace Honamic.Framework.Facade.FastCrud.Web.Controllers;

public abstract class CrudController<TEntity, TEntityDto> :
  CrudController<TEntity, TEntityDto, long>
  where TEntity : Entity<long>
  where TEntityDto : EntityDto<TEntityDto, TEntity, long>
{

    public CrudController(ICrudEntityFacade<TEntity, TEntityDto> crudEntityFacade)
        : base(crudEntityFacade)
    {
    }
}

public abstract class CrudController<TEntity, TEntityDto, TPrimaryKey> :
    CrudController<TEntity, TEntityDto, TPrimaryKey, TEntityDto>
    where TEntity : Entity<TPrimaryKey>
    where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>

{

    public CrudController(ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey> crudEntityFacade)
        : base(crudEntityFacade)
    {
    }
}

public abstract class CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto> :
CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, PagedQueryFilter>
where TEntity : Entity<TPrimaryKey>
where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
         where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>

{

    public CrudController(ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto> crudEntityFacade)
        : base(crudEntityFacade)
    {
    }
}

public abstract class CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput> :
CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TEntityDto, TEntityDto>
where TEntity : Entity<TPrimaryKey>
where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
         where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
where TGetAllInput : PagedQueryFilter
{

    public CrudController(ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput> crudEntityFacade)
        : base(crudEntityFacade)
    {
    }
}

public abstract class CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput> :
 CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TCreateInput>
 where TEntity : Entity<TPrimaryKey>
 where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
             where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
 where TGetAllInput : PagedQueryFilter
 where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
{

    public CrudController(ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput> crudEntityFacade)
        : base(crudEntityFacade)
    {
    }
}


public abstract class CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput> :
    CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TEntityDto>
    where TEntity : Entity<TPrimaryKey>
    where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
             where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
    where TGetAllInput : PagedQueryFilter
    where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
    where TUpdateInput : EntityDto<TUpdateInput, TEntity, TPrimaryKey>
{

    public CrudController(ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput> crudEntityFacade)
        : base(crudEntityFacade)
    {
    }
}

public abstract class CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult> :
    CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, PagedQueryFilter>
    where TEntity : Entity<TPrimaryKey>
    where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
             where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
    where TGetAllInput : PagedQueryFilter
    where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
    where TUpdateInput : EntityDto<TUpdateInput, TEntity, TPrimaryKey>
    where TSelectResult : EntityDto<TSelectResult, TEntity, TPrimaryKey>
{

    public CrudController(ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult> crudEntityFacade)
        : base(crudEntityFacade)
    {
    }
}