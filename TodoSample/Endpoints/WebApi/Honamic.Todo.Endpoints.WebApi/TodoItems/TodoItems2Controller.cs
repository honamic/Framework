using Honamic.Framework.Applications.Results;
using Honamic.Framework.Commands;
using Honamic.Framework.Queries;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Query.Domain.TodoItems.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Honamic.Todo.Endpoints.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItems2Controller : ControllerBase
{
    private readonly ILogger<TodoItemsController> _logger;
    private readonly ICommandBus _commandBus;
    private readonly IQueryBus _queryBus;

    public TodoItems2Controller(ILogger<TodoItemsController> logger, ICommandBus commandBus, IQueryBus queryBus)
    {
        _logger = logger;
        _commandBus = commandBus;
        _queryBus = queryBus;
    }

    [HttpPost]
    public async Task<Result<CreateTodoItem2ResultCommand>> Post([FromBody] CreateTodoItem2Command model, CancellationToken cancellationToken)
    {
        return await _commandBus
            .DispatchAsync<CreateTodoItem2Command, Result<CreateTodoItem2ResultCommand>>
            (model, cancellationToken);
    }

    [HttpGet]
    public async Task<Result<PagedQueryResult<GetAllTodoItemsQueryResult>>> GetAll([FromQuery] GetAllTodoItemsQueryFilter model, CancellationToken cancellationToken)
    {
        return await _queryBus.Dispatch(model, cancellationToken);
    }
 
}
