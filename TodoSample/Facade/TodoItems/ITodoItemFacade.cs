using Honamic.Framework.Applications.Results;
using Honamic.Framework.Facade;
using Honamic.Todo.Application.TodoItems.Commands;

namespace Honamic.Todo.Facade.TodoItems;

public interface ITodoItemFacade : IBaseFacade
{
    Task<Result<long>> Create(CreateTodoItemCommand model, CancellationToken cancellationToken);
    Task<Result> MakeCompleted(MakeCompletedTodoItemCommand model, CancellationToken cancellationToken);

    Task<Result> Delete(DeleteTodoItemCommand model, CancellationToken cancellationToken);
}
