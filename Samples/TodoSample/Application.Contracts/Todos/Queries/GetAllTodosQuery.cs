using Honamic.Framework.Application.Authorizes;
using Honamic.Framework.Application.Results;
using Honamic.Framework.Queries;

namespace TodoSample.Application.Contracts.Todos.Queries;

[DynamicPermission(
    DisplayName = "لیست تسک ها",
    Group = "Todos",
    Module = null,
    Name = null,
    Description = "")]
public class GetAllTodosQuery : PagedQueryFilter, IQuery<Result<PagedQueryResult<GetAllTodosQueryResult>>>
{
    protected override string DefaultOrderBy => OrderByDesc("Id");
}


public class GetAllTodosQueryResult
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
}