
namespace Honamic.IdentityPlus.Domain.Roles;
public partial class Role
{
    /// <summary>
    /// Gets or sets the name for this role.
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the normalized name for this role.
    /// </summary>
    public virtual string? NormalizedName { get; set; }

    /// <summary>
    /// A random value that should change whenever a role is persisted to the store
    /// </summary>
    public virtual string? ConcurrencyStamp { get; set; }

    /// <summary>
    /// Returns the name of the role.
    /// </summary>
    /// <returns>The name of the role.</returns>
    public override string ToString()
    {
        return Name ?? string.Empty;
    }
}