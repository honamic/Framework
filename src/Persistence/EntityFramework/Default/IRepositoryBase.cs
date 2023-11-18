using Honamic.Framework.Domain;
using System.Linq.Expressions;

namespace Honamic.Framework.Persistence.EntityFramework;

public interface IRepositoryBase<TEntity, TKey>
    where TEntity : AggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IList<TEntity>> GetAllAsync();

    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> GetAsync(TKey id);

    Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

    Task InsertAsync(TEntity entity);

    Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate);

    void Remove(TEntity entity);
}