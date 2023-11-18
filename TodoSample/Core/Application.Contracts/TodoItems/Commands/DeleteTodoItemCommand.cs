using Honamic.Framework.Commands;

namespace Honamic.Todo.Application.TodoItems.Commands;
public record DeleteTodoItemCommand(long id) : ICommand;
