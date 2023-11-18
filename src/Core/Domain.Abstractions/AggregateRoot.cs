using Honamic.Framework.Domain.Audits;
using System.ComponentModel.DataAnnotations.Schema;

namespace Honamic.Framework.Domain;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAuditCreateSources, IAuditUpdateSources, IAggregateRoot
{
    private List<DomainEvent> _events= new List<DomainEvent>();
    private bool _markAsDeleted;
    private long _version;

    public long Version
    {
        get => _version;
        set => UpdateVersion(value);
    }

    [NotMapped]
    public IList<DomainEvent> Events => _events;

    public string? ModifiedSources { get; set; }

    public string? CreatedSources { get; set; }

    public void RaiseEvent(DomainEvent @event)
    {
        @event?.SetAggregateVersion(Version);

        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events?.Clear();
    }

    public void MarkAsDelete()
    {
        _markAsDeleted = true;
    }

    public bool IsMarkAsDeleted()
    {
        return _markAsDeleted;
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