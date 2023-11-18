namespace Honamic.Framework.Domain.Audits;

public interface IAuditUpdateSources
{
    public string? ModifiedSources { get; set; }
}