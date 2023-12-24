using Honamic.Framework.Domain;

namespace Honamic.IdentityPlus.Domain.Roles;

public partial class RoleClaim : Entity<long>, IEquatable<long>
{
    public bool Equals(long other)
    {
        return Id == other;
    }
}
