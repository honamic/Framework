using Honamic.Framework.Application.Results;
using Honamic.Framework.Commands;
using Honamic.Framework.Domain;
using TodoSample.Application.Contracts.Todos.Commands;
using TodoSample.Domain.Todos;

namespace TodoSample.Application.Todos.CommandHandlers;

public class CreateTodoCommandHandler : ICommandHandler<CreateTodoCommand, Result<CreateTodoCommandResult>>
{
    private readonly IIdGenerator _idGenerator;
    private readonly ITodoRepository _todoRepository;

    public CreateTodoCommandHandler(ITodoRepository todoRepository, IIdGenerator idGenerator)
    {
        _todoRepository = todoRepository;
        _idGenerator = idGenerator;
    }

    public async Task<Result<CreateTodoCommandResult>> HandleAsync(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var exist = await _todoRepository.ExistsByTitleAsync(command.Title.Trim(), null);

        if (exist)
        {
            return Result<CreateTodoCommandResult>.Failure(ResultStatus.ValidationFailed, "این عنوان قبلا استفاده شده است.");
        }

        var newTodo = Todo.Create(_idGenerator.GetNewId(), command.Title.Trim(), command.Description);

        await _todoRepository.InsertAsync(newTodo, cancellationToken);

        return new CreateTodoCommandResult { Id = newTodo.Id };
    }
}