namespace Honamic.Framework.Domain;

public interface IAggregateRoot
{
    IList<DomainEvent> Events { get; }

    void ClearEvents();

    void MarkAsDelete();

    bool IsMarkAsDeleted();
}