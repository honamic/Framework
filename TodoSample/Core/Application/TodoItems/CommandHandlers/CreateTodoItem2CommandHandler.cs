using Honamic.Framework.Applications.Results;
using Honamic.Framework.Commands;
using Honamic.Framework.Domain;
using Honamic.Todo.Application.TodoItems.Commands;
using Honamic.Todo.Domain.TodoItems;

namespace Honamic.Todo.Application.TodoItems.CommandHandlers;
internal class CreateTodoItem2CommandHandler :
    ICommandHandler<CreateTodoItem2Command, Result<CreateTodoItem2ResultCommand>>
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IIdGenerator _idGenerator;

    public CreateTodoItem2CommandHandler(ITodoItemRepository todoItemRepository, IIdGenerator idGenerator)
    {
        _todoItemRepository = todoItemRepository;
        _idGenerator = idGenerator;
    }

    public async Task<Result<CreateTodoItem2ResultCommand>> HandleAsync(CreateTodoItem2Command command, CancellationToken cancellationToken)
    {
        var todoItem = new TodoItem(_idGenerator.GetNewId(), command.title, command.content, command.tags);
        await _todoItemRepository.InsertAsync(todoItem);

        var result = new CreateTodoItem2ResultCommand
        {
            Id = todoItem.Id
        };

        return result;
    }

}
