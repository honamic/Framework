using Honamic.IdentityPlus.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Honamic.IdentityPlus.Application;

public class IdentityPlusRoleManager : RoleManager<Role>
{
    public IdentityPlusRoleManager(IRoleStore<Role> store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleManager<Role>> logger)
        : base(store, roleValidators, keyNormalizer, errors, logger)
    {

    }
}