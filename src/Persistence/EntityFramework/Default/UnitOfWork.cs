using Honamic.Framework.Domain;
using Honamic.Framework.Events;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Honamic.Framework.Persistence.EntityFramework;

internal class UnitOfWork : IUnitOfWork
{
    private readonly DbContext Context;
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;
    private readonly IEnumerable<IUnitOfWorkInterceptor> _unitOfWorkInterceptors;

    public UnitOfWork(DbContext context,
        IDomainEventsDispatcher domainEventsDispatcher,
        IEnumerable<IUnitOfWorkInterceptor> unitOfWorkInterceptors)
    {
        Context = context;
        _domainEventsDispatcher = domainEventsDispatcher;
        _unitOfWorkInterceptors = unitOfWorkInterceptors;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Context.Database.CurrentTransaction == null)
        {
            await Context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            await AfterBeginTransactionAsync(cancellationToken);
        }
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        await BeforeSaveChangesAsync(cancellationToken);
        await _domainEventsDispatcher.DispatchEventsAsync();
        await Context.SaveChangesAsync(cancellationToken);
        await AfterSaveChangesAsync(cancellationToken);

    }

    public virtual async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Context.Database.CurrentTransaction == null)
        {
            throw new InvalidOperationException("there is no external transaction");
        }

        await Context.Database.CurrentTransaction.CommitAsync(cancellationToken);
        await AfterCommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Context.Database.CurrentTransaction != null)
        {
            await Context.Database.CurrentTransaction.RollbackAsync(cancellationToken);
            await AfterRollbackTransactionAsync(cancellationToken);
        }
    }

    #region interceptor
    private Task BeforeSaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(_unitOfWorkInterceptors.Select(c => c.BeforeSaveChangesAsync(cancellationToken)));
    }

    private Task AfterSaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(_unitOfWorkInterceptors.Select(c => c.AfterSaveChangesAsync(cancellationToken)));
    }

    private Task AfterBeginTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(_unitOfWorkInterceptors.Select(c => c.AfterBeginTransactionAsync(cancellationToken)));
    }


    private Task AfterCommitTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(_unitOfWorkInterceptors.Select(c => c.AfterCommitTransactionAsync(cancellationToken)));
    }

    private Task AfterRollbackTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(_unitOfWorkInterceptors.Select(c => c.AfterRollbackTransactionAsync(cancellationToken)));
    }
    #endregion

}
