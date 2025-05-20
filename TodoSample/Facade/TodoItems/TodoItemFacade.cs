using Honamic.Framework.Applications.Results;
using Honamic.Framework.Commands;
using Honamic.Framework.Events;
using Honamic.Framework.Facade;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Domain.TodoItems;
using Microsoft.Extensions.Logging;

namespace Honamic.Todo.Facade.TodoItems;

[FacadeDynamicAuthorize]
internal class TodoItemFacade : BaseFacade, ITodoItemFacade
{
    private readonly ICommandBus _commandBus;
    private readonly IEventBus _eventBus;
    public TodoItemFacade(ILogger<TodoItemFacade> logger, ICommandBus commandBus, IEventBus eventBus) : base(logger)
    {
        _commandBus = commandBus;
        _eventBus = eventBus;
    }

    public async Task<Result<long>> Create(CreateTodoItemCommand model, CancellationToken cancellationToken)
    {
        long result = -1;

        _eventBus.On((TodoItemCreatedEvent @event) =>
        {
            result = @event.AggregateId;
        });

        await _commandBus.DispatchAsync(model, cancellationToken);

        return result;
    }

    public async Task<Result> MakeCompleted(MakeCompletedTodoItemCommand model, CancellationToken cancellationToken)
    {
        await _commandBus.DispatchAsync(model, cancellationToken);
        return ResultStatus.Ok;
    }

    public async Task<Result> Delete(DeleteTodoItemCommand model, CancellationToken cancellationToken)
    {
        await _commandBus.DispatchAsync(model, cancellationToken);

        return ResultStatus.Ok;
    }
}