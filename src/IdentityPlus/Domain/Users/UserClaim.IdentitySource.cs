using System.Security.Claims;

namespace Honamic.IdentityPlus.Domain.Users;

public partial class UserClaim
{
    public virtual long UserId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    public virtual string? ClaimType { get; set; }

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    public virtual string? ClaimValue { get; set; }

    /// <summary>
    /// Converts the entity into a Claim instance.
    /// </summary>
    /// <returns></returns>
    public virtual Claim ToClaim()
    {
        return new Claim(ClaimType!, ClaimValue!);
    }

    /// <summary>
    /// Reads the type and value from the Claim.
    /// </summary>
    /// <param name="claim"></param>
    public virtual void InitializeFromClaim(Claim claim)
    {
        ClaimType = claim.Type;
        ClaimValue = claim.Value;
    }
}