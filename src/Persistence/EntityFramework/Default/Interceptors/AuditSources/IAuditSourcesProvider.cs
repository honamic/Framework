namespace Honamic.Framework.Persistence.EntityFramework.Interceptors.AuditSources;

public interface IAuditSourcesProvider
{
    AuditSourceValues GetAuditSourceValues();
}