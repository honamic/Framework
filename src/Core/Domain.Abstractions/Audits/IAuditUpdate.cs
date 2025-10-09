namespace Honamic.Framework.Domain.Audits;

public interface IAuditUpdate
{
    public string? ModifiedBy { get; }

    public DateTimeOffset? ModifiedOn { get; }
}
