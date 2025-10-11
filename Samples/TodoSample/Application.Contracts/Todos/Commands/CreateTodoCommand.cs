using Honamic.Framework.Application.Authorizes;
using Honamic.Framework.Application.Results;
using Honamic.Framework.Commands;

namespace TodoSample.Application.Contracts.Todos.Commands;

[DynamicPermission(
    DisplayName = "ایجاد تسک جدید",
    Group = "Todos",
    Module = null,
    Name = null,
    Description = "")]
public class CreateTodoCommand : ICommand<Result<CreateTodoCommandResult>>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}

public class CreateTodoCommandResult
{
    public long Id { get; set; }
}