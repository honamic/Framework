namespace Honamic.Framework.Domain.Audits;

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public sealed class AuditMaskAttribute : Attribute
{
    /// <summary>Replacement text (default: *****).</summary>
    public string Mask { get; set; } = "*****";

    /// <summary>
    /// Optional regex pattern. When set, only matched parts are replaced with <see cref="Mask"/>.
    /// When null, the entire value is replaced.
    /// Example: Pattern="\d(?=\d{4})" Mask="*" → keeps last 4 digits, masks the rest.
    /// </summary>
    public string? Pattern { get; set; }
}
