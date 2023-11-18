namespace Honamic.Framework.Domain.Audits;

public interface IAuditUpdate
{
    public string? ModifiedBy { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }
}
