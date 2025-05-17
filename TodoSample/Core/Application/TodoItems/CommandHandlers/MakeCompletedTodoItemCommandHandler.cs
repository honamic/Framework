using Honamic.Framework.Commands;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Domain.TodoItems;

namespace Honamic.Todo.Application.TodoItems.CommandHandlers;
internal class MakeCompletedTodoItemCommandHandler(ITodoItemRepository todoItemRepository) : ICommandHandler<MakeCompletedTodoItemCommand>
{

    public async Task HandleAsync(MakeCompletedTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoItem = await todoItemRepository.GetAsync(command.id);
        todoItem.MakeDone();
        await todoItemRepository.Update(todoItem);
    }
}
