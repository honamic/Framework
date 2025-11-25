using Honamic.Framework.Queries;
using TodoSample.Todos;

namespace TodoSample.QueryModels.Todos;

public class TodoQueryModel : AggregateRootQueryBase<long>
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public TodoStatus Status { get; set; }
    public DateTime? DoneAt { get; set; }
}