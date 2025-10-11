using Honamic.Framework.Application.Authorizes;
using Honamic.Framework.Application.Results;
using Honamic.Framework.Queries;

namespace TodoSample.Application.Contracts.Todos.Queries;


[DynamicPermission(
    DisplayName = "نمایش تسک جدید",
    Group = "Todos",
    Module = null,
    Name = null,
    Description = "")]
public class GetTodoQuery : IQuery<Result<GetTodoQueryResult?>>
{
    public long Id { get; set; }
}


public class GetTodoQueryResult
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
}