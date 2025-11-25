using Honamic.Framework.Application.Authorizes;
using Honamic.Framework.Application.Results;
using Honamic.Framework.Queries;
using Honamic.Framework.Queries.GridColumns;

namespace TodoSample.Application.Contracts.Todos.Queries;


[DynamicPermission(
    DisplayName = "نمایش تسک جدید",
    Feature = "Todos",
    Module = null,
    Name = null,
    Description = "")]
public class GetTodoQuery : IQuery<Result<GetTodoQueryResult?>>
{
    public long Id { get; set; }
}


public class GetTodoQueryResult : AggregateQueryResult<long>
{
    [GridColumn(Order = 0, Visible = true)]
    public override long Id { get => base.Id; set => base.Id = value; }

    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
}