using Honamic.Framework.Domain;

namespace Honamic.IdentityPlus.Domain.Users;

public partial class UserLogin : Entity<long>, IEquatable<long>
{
    public bool Equals(long other)
    {
        return Id == other;
    }
}