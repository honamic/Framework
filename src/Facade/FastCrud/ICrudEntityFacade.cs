using Honamic.Framework.Domain;
using Honamic.Framework.Facade.FastCrud.Dtos;
using Honamic.Framework.Facade.Results;
using Honamic.Framework.Queries;

namespace Honamic.Framework.Facade.FastCrud;

public interface ICrudEntityFacade<TEntity, TEntityDto> :
 ICrudEntityFacade<TEntity, TEntityDto, long>
    where TEntity : Entity<long>
    where TEntityDto : EntityDto<TEntityDto, TEntity, long>
{

}
public interface ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey> :
  ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntityDto>
     where TEntity : Entity<TPrimaryKey>
     where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
{

}

public interface ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto> :
  ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, PagedQueryFilter>
     where TEntity : Entity<TPrimaryKey>
     where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
     where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
{

}

public interface ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput> :
      ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TEntityDto>
 where TEntity : Entity<TPrimaryKey>
 where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
    where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
 where TGetAllInput : PagedQueryFilter
{

}
public interface ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput> :
      ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TCreateInput>
 where TEntity : Entity<TPrimaryKey>
 where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
    where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
 where TGetAllInput : PagedQueryFilter
 where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
{

}

public interface ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput> :
     ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TEntityDto>
     where TEntity : Entity<TPrimaryKey>
     where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
    where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
     where TGetAllInput : PagedQueryFilter
     where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
     where TUpdateInput : EntityDto<TUpdateInput, TEntity, TPrimaryKey>
{

}

public interface ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult> :
       ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, PagedQueryFilter>
       where TEntity : Entity<TPrimaryKey>
       where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
       where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
       where TGetAllInput : PagedQueryFilter
       where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
       where TUpdateInput : EntityDto<TUpdateInput, TEntity, TPrimaryKey>
       where TSelectResult : EntityDto<TSelectResult, TEntity, TPrimaryKey>
{

}

public interface ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, TSelectInput>
    where TEntity : Entity<TPrimaryKey>
    where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
    where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
    where TGetAllInput : PagedQueryFilter
    where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
    where TUpdateInput : EntityDto<TUpdateInput, TEntity, TPrimaryKey>
    where TSelectResult : EntityDto<TSelectResult, TEntity, TPrimaryKey>
    where TSelectInput : PagedQueryFilter

{
    Task<Result<TEntityDto>> CreateAsync(TCreateInput input, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken);
    Task<Result<PagedQueryResult<TEntitiesDto>>> GetAllAsync(TGetAllInput input, CancellationToken cancellationToken);
    Task<Result<TEntityDto>> GetAsync(TPrimaryKey id, CancellationToken cancellationToken);
    Task<Result<TEntityDto>> UpdateAsync(TUpdateInput input, CancellationToken cancellationToken);
    Task<Result<PagedQueryResult<TSelectResult>>> GetSelectAsync(TSelectInput input, CancellationToken cancellationToken);
}