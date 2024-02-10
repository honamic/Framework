using Honamic.Framework.Facade;
using Honamic.Framework.Facade.Results;
using Honamic.Framework.Queries;
using Honamic.IdentityPlus.Application.Users.Queries;

namespace Honamic.IdentityPlus.Facade.Accounts;

public interface IUserFacade : IBaseFacade
{
    Task<Result<PagedQueryResult<GetAllUsersQueryResult>>> GetAllUsers(GetAllUsersQueryFilter filter, CancellationToken cancellationToken);
}