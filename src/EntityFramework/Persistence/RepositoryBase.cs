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

    public virtual Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return DbSet.AddAsync(entity, cancellationToken).AsTask();
    }

    public virtual void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return DbSet.CountAsync(predicate, cancellationToken);
    }

    public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetQuery().SingleOrDefaultAsync(CreateEqualityExpressionForId<TEntity, TKey>(id), cancellationToken);
        
        if (entity is null)
        {
            throw new NotFoundBusinessException($"Entity of type {typeof(TEntity).Name} with id {id} was not found.");
        }
        
        return entity;
    }

    public virtual Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return GetQuery().FirstOrDefaultAsync(CreateEqualityExpressionForId<TEntity, TKey>(id), cancellationToken);
    }

    public virtual Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return GetQuery().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await GetQuery().Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await GetQuery().ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return GetQuery().AnyAsync(predicate, cancellationToken);
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
