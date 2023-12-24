using Honamic.Framework.Domain.Audits;
using System.ComponentModel.DataAnnotations;

namespace Honamic.Framework.Domain;
public abstract class Entity<TKey> : IAuditCreate, IAuditUpdate
{
    public virtual TKey Id { get; set; } = default!;

    [StringLength(100)]
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }

    [StringLength(100)]
    public string? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != GetType()) return false;
        var otherEntity = obj as Entity<TKey>;
        return otherEntity != null && Id != null && Id.Equals(otherEntity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}