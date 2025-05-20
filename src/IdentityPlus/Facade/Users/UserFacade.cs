using Honamic.Framework.Applications.Results;
using Honamic.Framework.Commands;
using Honamic.Framework.Facade;
using Honamic.Framework.Queries;
using Honamic.IdentityPlus.Application.Users.Queries;
using Microsoft.Extensions.Logging;

namespace Honamic.IdentityPlus.Facade.Accounts;
public class UserFacade(ILogger<AccountFacade> logger, ICommandBus commandBus, IQueryBus queryBus) : BaseFacade(logger), IUserFacade
{
 
    public async Task<Result<PagedQueryResult<GetAllUsersQueryResult>>> GetAllUsers(GetAllUsersQueryFilter filter, CancellationToken cancellationToken)
    {
        var result = await queryBus.Dispatch<GetAllUsersQueryFilter, PagedQueryResult<GetAllUsersQueryResult>>(filter, cancellationToken);

        return result;
    }

}
