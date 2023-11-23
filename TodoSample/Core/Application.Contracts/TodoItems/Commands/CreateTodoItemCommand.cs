
using Honamic.Framework.Commands;

namespace Honamic.Todo.Application.TodoItems.Commands;
public record CreateTodoItemCommand(string title, string content, List<string> tags) : ICommand;
