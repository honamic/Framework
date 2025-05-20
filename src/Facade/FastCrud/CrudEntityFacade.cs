using AutoMapper;
using AutoMapper.QueryableExtensions;
using Honamic.Framework.Domain;
using Honamic.Framework.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using Honamic.Framework.Utilities.Extensions;
using Honamic.Framework.Facade.FastCrud.Dtos;
using Honamic.Framework.Applications.Results;

namespace Honamic.Framework.Facade.FastCrud;

public abstract class CrudEntityFacade<TEntity, TEntityDto, TPrimaryKey,
        TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, TSelectInput> :

    BaseEntityFacade<TEntity>,
    ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey,
        TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, TSelectInput>
    where TEntity : Entity<TPrimaryKey>
    where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
    where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
    where TUpdateInput : EntityDto<TUpdateInput, TEntity, TPrimaryKey>
    where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
    where TGetAllInput : PagedQueryFilter
    where TSelectResult : EntityDto<TSelectResult, TEntity, TPrimaryKey>
    where TSelectInput : PagedQueryFilter
{
    protected const string UnhandledExceptionMessage = "An unhandled exception has been occurred.";
    private readonly Lazy<IMapper> _mapper;
    protected IMapper Mapper => _mapper.Value;

    protected CrudEntityFacade(ILogger logger, FastCrudDbContext dbContext, IServiceProvider serviceProvider)
        : base(logger, dbContext)
    {
        _mapper = new Lazy<IMapper>(() => serviceProvider.GetRequiredService<IMapper>());
    }

    public virtual async Task<Result<PagedQueryResult<TEntitiesDto>>> GetAllAsync(TGetAllInput input, CancellationToken cancellationToken)
    {
        var result = new Result<PagedQueryResult<TEntitiesDto>>();

        try
        {
            var query = GetAllQuery(input);

            query = await PrepareGetAllQuery(input, result, query);

            if (query == null)
            {
                return result;
            }

            if (!input.OrderBy.HasValue())
                input.OrderBy = "Id";

            var EntitiesDto = await query.ProjectTo<TEntitiesDto>(Mapper.ConfigurationProvider)
                                           .ToPagedListAsync(input, cancellationToken);

            if (!await FinalizeGetAll(input, EntitiesDto, result))
            {
                return result;
            }

            result.Data = EntitiesDto;

            result.Succeed();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, UnhandledExceptionMessage);
            result.SetStatusAsUnhandledExceptionWithSorryError();
        }

        return result;
    }

    public virtual async Task<Result<TEntityDto>> GetAsync(TPrimaryKey input, CancellationToken cancellationToken)
    {
        var result = new Result<TEntityDto>();

        try
        {
            var query = GetQuery(input);

            query = await PrepareGetQuery(input, result, query);

            if (query == null)
            {
                result.SetStatusAsNotFound();
                return result;
            }

            var getEntity = await query.ProjectTo<TEntityDto>(Mapper.ConfigurationProvider)
                            .SingleOrDefaultAsync(cancellationToken);

            if (getEntity == null)
            {
                result.SetStatusAsNotFound("چیزی یافت نشد.");
                //result.SetStatusAs404NotFound("Entity not found.");
                return result;
            }

            if (!await FinalizeGet(input, getEntity, result))
            {
                return result;
            }
            result.Data = getEntity;

            result.Succeed();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, UnhandledExceptionMessage);
            result.SetStatusAsUnhandledExceptionWithSorryError();
        }

        return result;
    }

    public virtual async Task<Result<TEntityDto>> CreateAsync(TCreateInput input, CancellationToken cancellationToken)
    {
        var result = new Result<TEntityDto>();

        try
        {
            var newEntity = input.ToEntity(Mapper);

            if (!await PrepareCreate(input, newEntity, result))
            {
                return result;
            }

            Dbset.Add(newEntity);

            await CrudDbContext.SaveChangesAsync();

            var query = Dbset.AsQueryable().AsNoTracking();

            var newEntityDto = await query.ProjectTo<TEntityDto>(Mapper.ConfigurationProvider)
                           .SingleOrDefaultAsync(p => p.Id.Equals(newEntity.Id), cancellationToken);

            if (!await FinalizeCreate(input, newEntity, newEntityDto, result))
            {
                return result;
            }

            result.Data = newEntityDto;

            result.Succeed();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("Violation of UNIQUE KEY constraint", StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                result.SetStatusAsUnhandledException("کلید تکراری است.");
            }
            else
            {
                Logger.LogError(ex, UnhandledExceptionMessage);
                result.SetStatusAsUnhandledExceptionWithSorryError();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, UnhandledExceptionMessage);
            result.SetStatusAsUnhandledExceptionWithSorryError();
        }

        return result;
    }

    public virtual async Task<Result<TEntityDto>> UpdateAsync(TUpdateInput input, CancellationToken cancellationToken)
    {
        var result = new Result<TEntityDto>();

        try
        {
            var query = GetUpdateQuery(input);

            query = await PrepareUpdateQuery(input, result, query);

            if (query == null)
            {
                return result;
            }

            var currentEntity = await query.SingleOrDefaultAsync(cancellationToken);

            if (currentEntity == null)
            {
                result.SetStatusAsNotFound("چیزی یافت نشد.");
                return result;
            }

            if (!await PrepareUpdate(input, result, currentEntity))
            {
                return result;
            }

            input.ToEntity(Mapper, currentEntity);

            await CrudDbContext.SaveChangesAsync();

            var updatedEntityDto = await query.ProjectTo<TEntityDto>(Mapper.ConfigurationProvider)
                       .SingleOrDefaultAsync(p => p.Id.Equals(input.Id), cancellationToken);

            if (!await FinalizeUpdate(input, currentEntity, updatedEntityDto, result))
            {
                return result;
            }

            result.Data = updatedEntityDto;

            result.Succeed();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, UnhandledExceptionMessage);
            result.SetStatusAsUnhandledExceptionWithSorryError();
        }

        return result;
    }

    public virtual async Task<Result> DeleteAsync(TPrimaryKey input, CancellationToken cancellationToken)
    {
        var result = new Result();

        try
        {
            var query = GetDeleteQuery(input);

            query = await PrepareDeleteQuery(input, result, query);

            if (query == null)
            {
                return result;
            }

            var currentEntity = await query.SingleOrDefaultAsync(cancellationToken);

            if (currentEntity == null)
            {
                result.SetStatusAsNotFound("چیزی یافت نشد.");
                return result;
            }

            if (!await PrepareDelete(input, result, currentEntity))
            {
                return result;
            }

            Dbset.Remove(currentEntity);

            await CrudDbContext.SaveChangesAsync();

            if (!await FinalizeDelete(input, currentEntity, result))
            {
                return result;
            }

            result.Succeed("با موفقیت حذف شد.");
        }
        catch (DbUpdateException ex)
        {
            //var error1 = "The DELETE statement conflicted with the SAME TABLE REFERENCE constraint ";
            //var error2 = "The DELETE statement conflicted with the REFERENCE constraint ";

            if (ex.InnerException?
                .Message.Contains("The DELETE statement conflicted with the", StringComparison.InvariantCultureIgnoreCase)
                ?? false)
            {
                result.SetStatusAsInvalidDomainState("به علت استفاده در سیستم قابل حذف شدن نیست.");
            }
            else
            {
                Logger.LogError(ex, UnhandledExceptionMessage);
                result.SetStatusAsUnhandledExceptionWithSorryError();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, UnhandledExceptionMessage);
            result.SetStatusAsUnhandledExceptionWithSorryError();
        }

        return result;
    }

    public virtual async Task<Result<PagedQueryResult<TSelectResult>>> GetSelectAsync(TSelectInput input, CancellationToken cancellationToken)
    {
        var result = new Result<PagedQueryResult<TSelectResult>>();

        try
        {
            var query = GetSelectQuery(input);

            query = await PrepareSelectQuery(input, result, query);

            if (!input.OrderBy.HasValue())
                input.OrderBy = "Id";

            result.Data = await query.ProjectTo<TSelectResult>(Mapper.ConfigurationProvider)
                           .ToPagedListAsync(input, cancellationToken);

            result.Succeed();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, UnhandledExceptionMessage);
            result.SetStatusAsUnhandledExceptionWithSorryError();
        }

        return result;
    }

    #region Query

    protected IQueryable<TEntity> GetQuery(TPrimaryKey id)
    {
        return Dbset.AsQueryable().AsNoTracking().Where(p => p.Id.Equals(id));
    }

    protected IQueryable<TEntity> GetUpdateQuery(TUpdateInput input)
    {
        return Dbset.AsQueryable().Where(entity => entity.Id.Equals(input.Id));
    }

    protected IQueryable<TEntity> GetDeleteQuery(TPrimaryKey id)
    {
        return Dbset.AsQueryable().Where(entity => entity.Id.Equals(id));
    }

    protected IQueryable<TEntity> GetAllQuery(TGetAllInput input)
    {
        var query = Dbset.AsQueryable().AsNoTracking();

        query = ApplyKeywordToQuery(query, input);

        return query;
    }

    protected IQueryable<TEntity> GetSelectQuery<TSelectInput>(TSelectInput input) where TSelectInput : PagedQueryFilter
    {
        var query = Dbset.AsQueryable().AsNoTracking();

        query = ApplyKeywordToQuery(query, input);

        return query;
    }

    protected IQueryable<TEntity> ApplyKeywordToQuery(IQueryable<TEntity> query, PagedQueryFilter input)
    {
        if (!input.Keyword.HasValue())
        {
            return query;
        }

        var entityType = typeof(TEntity);
        var hasTitle = entityType.GetProperty("Title", typeof(string)) != null;
        var hasName = entityType.GetProperty("Name") != null;

        if (hasName && hasTitle)
        {
            query = query.Where("Name.Contains(@0) || Title.Contains(@0) ", input.Keyword);
        }
        else if (hasTitle)
        {
            query = query.Where("Title.Contains(@0) ", input.Keyword);
        }
        else if (hasName)
        {
            query = query.Where("Name.Contains(@0)", input.Keyword);
        }

        return query;
    }



    #endregion

    #region prepare

    protected virtual Task<IQueryable<TEntity>> PrepareGetAllQuery(TGetAllInput input, Result<PagedQueryResult<TEntitiesDto>> result, IQueryable<TEntity> query)
    {
        return Task.FromResult(query);
    }

    protected virtual Task<bool> PrepareCreate(TCreateInput input, TEntity newEntity, Result<TEntityDto> result)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<IQueryable<TEntity>> PrepareGetQuery(TPrimaryKey id, Result<TEntityDto> result, IQueryable<TEntity> query)
    {
        return Task.FromResult(query);
    }

    protected virtual Task<IQueryable<TEntity>> PrepareUpdateQuery(TUpdateInput input, Result<TEntityDto> result, IQueryable<TEntity> query)
    {
        return Task.FromResult(query);
    }

    protected virtual Task<bool> PrepareUpdate(TUpdateInput input, Result<TEntityDto> result, TEntity entity)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<IQueryable<TEntity>> PrepareDeleteQuery(TPrimaryKey id, Result result, IQueryable<TEntity> query)
    {
        return Task.FromResult(query);
    }

    protected virtual Task<bool> PrepareDelete(TPrimaryKey id, Result result, TEntity entity)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<IQueryable<TEntity>> PrepareSelectQuery<TSelectInput, TResult>
        (TSelectInput input, Result<PagedQueryResult<TResult>> result, IQueryable<TEntity> query)
    where TResult : IEntityDto<TPrimaryKey>
    where TSelectInput : PagedQueryFilter
    {
        return Task.FromResult(query);
    }


    #endregion

    #region Finalize

    protected virtual Task<bool> FinalizeGet(TPrimaryKey id, TEntityDto getEntity, Result<TEntityDto> result)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> FinalizeGetAll(TGetAllInput input, PagedQueryResult<TEntitiesDto> entitiesDto, Result<PagedQueryResult<TEntitiesDto>> result)
    {
        return Task.FromResult(true);
    }
    protected virtual Task<bool> FinalizeCreate(TCreateInput input, TEntity entity, TEntityDto entityDto, Result<TEntityDto> result)
    {
        return Task.FromResult(true);
    }

    protected virtual Task<bool> FinalizeUpdate(TUpdateInput input, TEntity Entity, TEntityDto updatedEntityDto, Result<TEntityDto> result)
    {
        return Task.FromResult(true);
    }
    protected virtual Task<bool> FinalizeDelete(TPrimaryKey id, TEntity deleteEntity, Result result)
    {
        return Task.FromResult(true);
    }
    #endregion
}