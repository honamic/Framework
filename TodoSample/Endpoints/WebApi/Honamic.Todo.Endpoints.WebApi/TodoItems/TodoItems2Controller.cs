using Honamic.Framework.Applications.Results;
using Honamic.Framework.Commands;
using Honamic.Todo.Application.TodoItems.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Honamic.Todo.Endpoints.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItems2Controller : ControllerBase
{
    private readonly ILogger<TodoItemsController> _logger;
    private readonly ICommandBus _commandBus;

    public TodoItems2Controller(ILogger<TodoItemsController> logger, ICommandBus commandBus)
    {
        _logger = logger;
        _commandBus = commandBus;
    }

    [HttpPost]
    public async Task<Result<CreateTodoItem2ResultCommand>> Post([FromBody] CreateTodoItem2Command model, CancellationToken cancellationToken)
    {
        return await _commandBus
            .DispatchAsync<CreateTodoItem2Command, Result<CreateTodoItem2ResultCommand>>
            (model, cancellationToken);
    }


}
