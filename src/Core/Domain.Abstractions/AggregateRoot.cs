using System.ComponentModel.DataAnnotations.Schema;

namespace Honamic.Framework.Domain;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
{
    private List<DomainEvent> _events = new List<DomainEvent>();
    private bool _markAsDeleted;
    private long _version;

    public long Version
    {
        get => _version;
        set => UpdateVersion(value);
    }

    [NotMapped]
    public IList<DomainEvent> Events => _events;

    public void RaiseEvent(DomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event, nameof(@event));

        @event.SetAggregateVersion(Version);

        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events?.Clear();
    }

    public bool IsMarkAsDeleted()
    {
        return _markAsDeleted;
    }

    internal void MarkAsDelete()
    {
        _markAsDeleted = true;
    }

    public void EnsureLatestVersion(long clientVersion)
    {
        if (clientVersion != Version)
            throw new ConcurrencyException(Version, clientVersion);
    }

    public virtual void Delete()
    {
        MarkAsDelete();
    }

    #region privates methods

    private void UpdateVersion(long value)
    {
        _version = value;
        UpdateEventVersions();
    }

    private void UpdateEventVersions()
    {
        if (_events == null)
            return;

        foreach (var _event in _events)
        {
            if (_event is DomainEvent domainEvent)
            {
                domainEvent.SetAggregateVersion(Version);
            }
        }
    }

    #endregion

}