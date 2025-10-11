using Honamic.Framework.Domain;
using Honamic.Framework.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
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

    protected override IList<Expression<Func<Todo, object?>>> GetIncludes()
    {
        return new List<Expression<Func<Todo, object?>>>
        {
             //c=>c.
        };
    }
}
