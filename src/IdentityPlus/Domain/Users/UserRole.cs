using Honamic.Framework.Domain;

namespace Honamic.IdentityPlus.Domain.Users;

public partial class UserRole : Entity<long>
{

}

// IdentitySource
public partial class UserRole 
{
    public virtual long UserId { get; set; } = default!;

    public virtual long RoleId { get; set; } = default!;
}