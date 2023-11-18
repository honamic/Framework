namespace Honamic.Framework.Domain.Audits;
public interface IAuditCreate
{
    string? CreatedBy { get; set; }

    DateTimeOffset? CreatedOn { get; set; }
}