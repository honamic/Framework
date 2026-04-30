using Honamic.Framework.Domain;
using Honamic.Framework.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoSample.Domain.Todos;

namespace TodoSample.Persistence.Todos;
internal class TodoRepository : RepositoryBase<Todo, long>, ITodoRepository
{

    public TodoRepository([FromKeyedServices(DomainConstants.PersistenceDbContextKey)] DbContext context) : base(context)
    {

    }

    public Task<bool> ExistsByTitleAsync(string title, long? excludeId)
    {
        if (excludeId.HasValue)
            return IsExistsAsync(c => c.Title == title && c.Id != excludeId.Value);
        else
            return IsExistsAsync(c => c.Title == title);
    }

    protected override IList<Func<IQueryable<Todo>, IQueryable<Todo>>> GetIncludes()
    {
        return new List<Func<IQueryable<Todo>, IQueryable<Todo>>>
        {
            // example: q => q.Include(c => c.Items).ThenInclude(i => i.SubItems)
        };
    }
}
