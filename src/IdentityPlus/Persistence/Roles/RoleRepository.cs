using Honamic.IdentityPlus.Domain.Roles;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Honamic.IdentityPlus.Persistence.Roles;

internal class RoleRepository : RoleStore<Role, DbContext, long, UserRole, RoleClaim>
{
    public RoleRepository(DbContext context, IdentityErrorDescriber? describer = null)
        : base(context, describer)
    {

    }
}
