using Honamic.Framework.Commands;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Domain.TodoItems;

namespace Honamic.Todo.Application.TodoItems.CommandHandlers;
internal class DeleteTodoItemCommandHandler(ITodoItemRepository _todoItemRepository) : ICommandHandler<DeleteTodoItemCommand>
{

    public async Task HandleAsync(DeleteTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetAsync(command.id);

        _todoItemRepository.Remove(todoItem);
    }
}
