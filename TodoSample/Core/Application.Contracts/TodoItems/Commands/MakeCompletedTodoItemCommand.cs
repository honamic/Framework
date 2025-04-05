
using Honamic.Framework.Commands;

namespace Honamic.Todo.Application.TodoItems.Commands;
public record MakeCompletedTodoItemCommand(long id) : ICommand;
