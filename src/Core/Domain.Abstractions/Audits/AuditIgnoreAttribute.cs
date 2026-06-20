namespace Honamic.Framework.Domain.Audits;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true)]
public sealed class AuditIgnoreAttribute : Attribute { }
