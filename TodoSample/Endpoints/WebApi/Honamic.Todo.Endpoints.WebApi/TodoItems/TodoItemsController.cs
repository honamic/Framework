using Honamic.Framework.Facade.Results;
using Honamic.Framework.Queries;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Facade.TodoItems;
using Honamic.Todo.Query.Domain.TodoItems.Models;
using Honamic.Todo.Query.Domain.TodoItems.Queries;
using Honamic.Todo.Query.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace Honamic.Todo.Endpoints.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly ILogger<TodoItemsController> _logger;
    private readonly ITodoItemFacade _todoItemFacade;
    private readonly ITodoItemQueryFacade _todoItemQueryFacade;

    public TodoItemsController(ITodoItemFacade todoItemService, ILogger<TodoItemsController> logger, ITodoItemQueryFacade todoItemQueryService)
    {
        _todoItemFacade = todoItemService;
        _logger = logger;
        _todoItemQueryFacade = todoItemQueryService;
    }

    [HttpGet]
    public Task<Result<PagedQueryResult<GetAllTodoItemsQueryResult>>> GetAll([FromQuery] GetAllTodoItemsQueryFilter model, CancellationToken cancellationToken)
    {
        return _todoItemQueryFacade.GetAllTodoItem(model, cancellationToken);
    }

    [HttpGet("{id}")]
    public Task<Result<TodoItemQuery>> Get(long id, CancellationToken cancellationToken)
    {
        return _todoItemQueryFacade.GetTodoItem(id, cancellationToken);

    }

    [HttpPost]
    public Task<Result<long>> Post([FromBody] CreateTodoItemCommand model, CancellationToken cancellationToken)
    {
        return _todoItemFacade.Create(model, cancellationToken);
    }

    [HttpPut("{id}")]
    public Task<Result> Put([FromRoute] long id, CancellationToken cancellationToken)
    {
        var cmd = new MakeCompletedTodoItemCommand(id);
        return _todoItemFacade.MakeCompleted(cmd, cancellationToken);
    }

    [HttpDelete("{id}")]
    public Task<Result> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        var command = new DeleteTodoItemCommand(id);
        return _todoItemFacade.Delete(command, cancellationToken);

    }
}
