using Honamic.Framework.Applications.Results;
using Honamic.Framework.Domain;
using Honamic.Framework.Facade.FastCrud.Dtos;
using Honamic.Framework.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Honamic.Framework.Facade.FastCrud.Web.Controllers;

[ApiController]
public abstract class CrudController<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, TSelectInput> : ControllerBase
    where TEntity : Entity<TPrimaryKey>
    where TEntityDto : EntityDto<TEntityDto, TEntity, TPrimaryKey>
             where TEntitiesDto : EntityDto<TEntitiesDto, TEntity, TPrimaryKey>
    where TGetAllInput : PagedQueryFilter
    where TCreateInput : EntityDto<TCreateInput, TEntity, TPrimaryKey>
    where TUpdateInput : EntityDto<TUpdateInput, TEntity, TPrimaryKey>
    where TSelectResult : EntityDto<TSelectResult, TEntity, TPrimaryKey>
    where TSelectInput : PagedQueryFilter
{


    private readonly ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, TSelectInput>
        _crudEntityService;

    public CrudController(ICrudEntityFacade<TEntity, TEntityDto, TPrimaryKey, TEntitiesDto, TGetAllInput, TCreateInput, TUpdateInput, TSelectResult, TSelectInput> crudEntityService)
    {
        _crudEntityService = crudEntityService;
    }

    [HttpGet]
    public virtual async Task<ActionResult<Result<PagedQueryResult<TEntitiesDto>>>> Get([FromQuery] TGetAllInput input, CancellationToken cancellationToken)
    {
        return await _crudEntityService.GetAllAsync(input, cancellationToken);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<Result<TEntityDto>>> Get([FromRoute] TPrimaryKey id, CancellationToken cancellationToken)
    {
        var result = await _crudEntityService.GetAsync(id, cancellationToken);

        return ResultToAction(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public virtual async Task<ActionResult<Result<TEntityDto>>> Create([FromBody] TCreateInput input, CancellationToken cancellationToken)
    {
        var result = await _crudEntityService.CreateAsync(input, cancellationToken);

        if (result.Status == ResultStatus.Ok)
        {
            return CreatedAtAction(
                 nameof(Get),
                 new { id = result.Data.Id },
         result);
        }

        return ResultToAction(result);
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<Result<TEntityDto>>> Update([FromRoute] TPrimaryKey id, [FromBody] TUpdateInput input, CancellationToken cancellationToken)
    {
        if (!id.Equals(input.Id))
        {
            ModelState.AddModelError("Id", "The identifier in the body does not match the path");

            return ( new Honamic.Framework.Endpoints.Web.Results.BadRequestResult(ModelState));
        }

        var result = await _crudEntityService.UpdateAsync(input, cancellationToken);


        return ResultToAction(result);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult<Result>> Delete([FromRoute] TPrimaryKey id, CancellationToken cancellationToken)
    {
        var result = await _crudEntityService.DeleteAsync(id, cancellationToken);


        return ResultToAction(result);
    }

    [HttpGet("select")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual Task<Result<PagedQueryResult<TSelectResult>>> GetSelect([FromQuery] TSelectInput input, CancellationToken cancellationToken)
    {
        return _crudEntityService.GetSelectAsync(input, cancellationToken);
    }


    private ActionResult<Result> ResultToAction(Result result)
    {
        switch (result.Status)
        {
            case ResultStatus.Ok:
                return Ok(result);

            case ResultStatus.Unauthorized:
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            case ResultStatus.Unauthenticated:
                return Unauthorized(result);

            case ResultStatus.UnhandledException:
                return new ObjectResult(result) { StatusCode= StatusCodes.Status500InternalServerError};

            case ResultStatus.ValidationError:
                return BadRequest(result);

            case ResultStatus.InvalidDomainState:
                return new ObjectResult(result) { StatusCode = StatusCodes.Status203NonAuthoritative };

            case ResultStatus.NotFound:
                return NotFound(result);

            default:
                return Ok(result);
        }
    }

    private ActionResult<Result<T>> ResultToAction<T>(Result<T> result)
    {
        switch (result.Status)
        {
            case ResultStatus.Ok:
                return Ok(result);
            
            case ResultStatus.Unauthorized:
            case ResultStatus.Unauthenticated:
                return Unauthorized(result);
            
            case ResultStatus.UnhandledException:
                return new ActionResult<Result<T>>(result);
           
            case ResultStatus.ValidationError:
            case ResultStatus.InvalidDomainState:
                return BadRequest(result);
            
            case ResultStatus.NotFound:
                return NotFound(result);
            
            default:
                return Ok(result);
        }
    }
}