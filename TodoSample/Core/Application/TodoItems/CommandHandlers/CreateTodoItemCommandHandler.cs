using Honamic.Framework.Commands;
using Honamic.Framework.Domain;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Domain.TodoItems;

namespace Honamic.Todo.Application.TodoItems.CommandHandlers;
internal class CreateTodoItemCommandHandler : ICommandHandler<CreateTodoItemCommand>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IIdGenerator _idGenerator;

    public CreateTodoItemCommandHandler(ITodoItemRepository todoItemRepository, IIdGenerator idGenerator)
    {
        _todoItemRepository = todoItemRepository;
        _idGenerator = idGenerator;
    }

    public async Task HandleAsync(CreateTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoItem = new TodoItem(_idGenerator.GetNewId(), command.title, command.content, command.tags);
        await _todoItemRepository.InsertAsync(todoItem);
    }
}
