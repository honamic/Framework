using Honamic.Framework.Facade;
using Honamic.Framework.Facade.Results;
using Honamic.Framework.Queries;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Honamic.Todo.Query.Domain.TodoItems.Models;
using Honamic.Todo.Query.Domain.TodoItems.Queries;
using Honamic.Todo.Query.Domain.TodoItems;

namespace Honamic.Todo.Facade.TodoItems;

[FacadeDynamicAuthorize]
[DisplayName("نمایش پیامک")]
public class TodoItemQueryFacade : BaseFacade, ITodoItemQueryFacade
{
    private readonly ITodoItemQueryRepository _TodoItemQueryRepository;

    public TodoItemQueryFacade(ITodoItemQueryRepository TodoItemQueryRepository, ILogger<TodoItemQueryFacade> logger) : base(logger)
    {
        _TodoItemQueryRepository = TodoItemQueryRepository;
    }

    [DisplayName("نمایش")]
    public async Task<Result<TodoItemQuery>> GetTodoItem(long id, CancellationToken cancellationToken)
    {
        return await _TodoItemQueryRepository.GetAsync(id, cancellationToken);
    }

    [DisplayName("نمایش همه")]
    public async Task<Result<PagedQueryResult<GetAllTodoItemsQueryResult>>> GetAllTodoItem(GetAllTodoItemsQueryFilter filter, CancellationToken cancellationToken)
    {
        return await _TodoItemQueryRepository.GetAllAsync(filter, cancellationToken);
    }

}