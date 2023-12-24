
namespace Honamic.IdentityPlus.Domain.Users;

public partial class UserLogin
{
    /// <summary>
    /// Gets or sets the login provider for the login (e.g. facebook, google)
    /// </summary>
    public virtual string LoginProvider { get; set; } = default!;

    /// <summary>
    /// Gets or sets the unique provider identifier for this login.
    /// </summary>
    public virtual string ProviderKey { get; set; } = default!;

    /// <summary>
    /// Gets or sets the friendly name used in a UI for this login.
    /// </summary>
    public virtual string? ProviderDisplayName { get; set; }

    /// <summary>
    /// Gets or sets the primary key of the user associated with this login.
    /// </summary>
    public virtual long UserId { get; set; } = default!;
}