using Honamic.IdentityPlus.Domain.Roles;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Honamic.IdentityPlus.Persistence.Users;
internal class UserRepository : UserStore<User, Role, DbContext, long, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
{
    public UserRepository(DbContext context, IdentityErrorDescriber? describer = null)
        : base(context, describer)
    {

    }
}
