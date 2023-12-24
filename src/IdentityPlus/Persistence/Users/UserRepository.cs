using Honamic.Framework.Domain;
using Honamic.IdentityPlus.Domain.Roles;
using Honamic.IdentityPlus.Domain.Users;
using Honamic.IdentityPlus.Persistence.IdentitySource;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Honamic.IdentityPlus.Persistence.Users;
internal class UserRepository : UserStore<User, Role, DbContext, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdGenerator _idGenerator;

    public UserRepository(DbContext context, IUnitOfWork unitOfWork, IIdGenerator idGenerator, IdentityErrorDescriber? describer = null)
        : base(context, describer)
    {
        _unitOfWork = unitOfWork;
        _idGenerator = idGenerator;
    }


    public override async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user.Id == 0)
        {
            user.SetManualId(_idGenerator.GetNewId());
        }
        var beforeAutoSaveChanges = AutoSaveChanges;
        AutoSaveChanges = false;
        
        var result = await base.CreateAsync(user, cancellationToken);
        
        if (!result.Succeeded)
        {
            return result;
        }

        AutoSaveChanges = beforeAutoSaveChanges;

        await _unitOfWork.SaveChangesAsync();

        return result;
    }
}
