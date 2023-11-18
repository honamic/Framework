using Honamic.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Honamic.Framework.Persistence.EntityFramework;

public abstract class RepositoryBase<TEntity, TKey>
    : IRepositoryBase<TEntity, TKey>
    where TEntity : AggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    protected DbContext Context;
    protected DbSet<TEntity> DbSet;

    protected RepositoryBase(DbContext context)
    {
        DbSet = context.Set<TEntity>();
        Context = context;
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.CountAsync(predicate);
    }

    public virtual async Task<TEntity> GetAsync(TKey id)
    {
        return await GetQuery().SingleAsync(RepositoryBase<TEntity, TKey>.CreateEqualityExpressionForId<TEntity, TKey>(id));
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await GetQuery().FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await GetQuery().Where(predicate).ToListAsync();
    }

    public virtual async Task<IList<TEntity>> GetAllAsync()
    {
        return await GetQuery().ToListAsync();
    }

    public virtual async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await GetQuery().AnyAsync(predicate);
    }

    protected virtual IQueryable<TEntity> GetQuery()
    {
        var query = DbSet.AsQueryable();

        var includes = GetIncludes();

        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                if (include.Body.NodeType == ExpressionType.Constant)
                {
                    var memberExpression = include.Body as ConstantExpression;
                    query = query.Include(memberExpression.Value.ToString());
                }
                else
                {
                    query = query.Include(include);
                }
            }
        }

        return query;
    }

    protected abstract IList<Expression<Func<TEntity, object?>>> GetIncludes();


    private static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId<TEntity, TKey>(TKey id)
       where TEntity : Entity<TKey>
    {
        var lambdaParam = Expression.Parameter(typeof(TEntity));
        var lambdaBody = Expression.Equal(
            Expression.PropertyOrField(lambdaParam, nameof(Entity<TKey>.Id)),
            Expression.Constant(id, typeof(TKey))
        );

        return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
    }
}
