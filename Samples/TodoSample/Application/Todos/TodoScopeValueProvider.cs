using Honamic.Framework.Application.Authorizes;
using TodoSample.QueryModels.Todos;

namespace TodoSample.Application.Todos;

public class TodoScopeValueProvider : IScopeValueProvider
{
    private readonly ITodoQueryModelRepository _todoQueryModelRepository;

    public TodoScopeValueProvider(ITodoQueryModelRepository todoQueryModelRepository)
    {
        _todoQueryModelRepository = todoQueryModelRepository;
    }

    public string Key => "Todo";

    public async Task<List<ScopeValueItem>> ResolveAsync(IReadOnlyCollection<string> values, CancellationToken cancellationToken)
    {
        var ids = values
           .Select(v => long.TryParse(v, out var id) ? id : (long?)null)
           .Where(id => id.HasValue)
           .Select(id => id!.Value)
           .ToArray();

        if (ids.Length == 0)
        {
            return new List<ScopeValueItem>();
        }
        long sampleId = ids.First();
        var sampleData = await _todoQueryModelRepository.GetAsync(new Contracts.Todos.Queries.GetTodoQuery { Id = sampleId }, cancellationToken);

        return new List<ScopeValueItem>
        {
             new ScopeValueItem
             {
                    Value = sampleData.Id.ToString(),
                    DisplayValue = sampleData.Title,
             }
        };
    }

    public async Task<ScopeValuePage> SearchAsync(ScopeValueQuery query, CancellationToken cancellationToken)
    {
        var result = await _todoQueryModelRepository.GetAll(new Contracts.Todos.Queries.GetAllTodosQuery
        {
            Keyword = query.Search,
            Page = query.Page,
            PageSize = query.PageSize,

        }, cancellationToken);

        return new ScopeValuePage
        {
            Items = result.Items.Select(c => new ScopeValueItem
            {
                Value = c.Id.ToString(),
                DisplayValue = c.Title,
            }).ToList(),
            Total = (int)result.TotalItems,
        };
    }
}
