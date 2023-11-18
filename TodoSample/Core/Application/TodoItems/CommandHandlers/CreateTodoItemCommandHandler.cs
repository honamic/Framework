using Honamic.Framework.Commands;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Domain.TodoItems;

namespace Honamic.Todo.Application.TodoItems.CommandHandlers;
internal class CreateTodoItemCommandHandler : ICommandHandler<CreateTodoItemCommand>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public CreateTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task HandleAsync(CreateTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoItem = new TodoItem(command.id, command.title, command.content, command.tags);
        await _todoItemRepository.InsertAsync(todoItem);
    }
}
