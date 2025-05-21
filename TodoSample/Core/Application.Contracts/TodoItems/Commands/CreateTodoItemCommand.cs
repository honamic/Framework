
using Honamic.Framework.Applications.Results;
using Honamic.Framework.Commands;

namespace Honamic.Todo.Application.TodoItems.Commands;
public record CreateTodoItemCommand(string title, string content, List<string> tags) : ICommand;




public record CreateTodoItem2Command(string title, string content, List<string> tags)
    : ICommand<Result<CreateTodoItem2ResultCommand>>;



public class CreateTodoItem2ResultCommand
{
    public long Id { get; set; }
}