
namespace Honamic.IdentityPlus.Domain.Users;

public partial class UserToken 
{
    /// <summary>
    /// Gets or sets the primary key of the user that the token belongs to.
    /// </summary>
    public virtual long UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the LoginProvider this token is from.
    /// </summary>
    public virtual string LoginProvider { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the token.
    /// </summary>
    public virtual string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the token value.
    /// </summary>
    [IdentityPlusProtectedPersonalData]
    public virtual string? Value { get; set; }
}
