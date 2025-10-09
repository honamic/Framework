namespace Honamic.Framework.Domain.Audits;
public interface IAuditCreate
{
    string? CreatedBy { get; }

    DateTimeOffset? CreatedOn { get; }
}