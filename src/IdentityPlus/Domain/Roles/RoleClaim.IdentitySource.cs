using System.Security.Claims;

namespace Honamic.IdentityPlus.Domain.Roles;

public partial class RoleClaim 
{
    public virtual long RoleId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    public virtual string? ClaimType { get; set; }

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    public virtual string? ClaimValue { get; set; }

    /// <summary>
    /// Constructs a new claim with the type and value.
    /// </summary>
    /// <returns>The <see cref="Claim"/> that was produced.</returns>
    public virtual Claim ToClaim()
    {
        return new Claim(ClaimType!, ClaimValue!);
    }

    /// <summary>
    /// Initializes by copying ClaimType and ClaimValue from the other claim.
    /// </summary>
    /// <param name="other">The claim to initialize from.</param>
    public virtual void InitializeFromClaim(Claim? other)
    {
        ClaimType = other?.Type;
        ClaimValue = other?.Value;
    }
}
