using Honamic.Framework.Facade.Results;
using Honamic.Framework.Queries;
using Honamic.IdentityPlus.Application.Users.Queries;
using Honamic.IdentityPlus.Facade.Users;
using Microsoft.AspNetCore.Mvc;

namespace Honamic.IdentityPlus.WebApi.Users;


[Route("api/Users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserFacade _userFacade;

    public UserController(IUserFacade userFacade)
    {
        _userFacade = userFacade;
    }


    [HttpGet()]
    public Task<Result<PagedQueryResult<GetAllUsersQueryResult>>> AllUsers([FromQuery] GetAllUsersQueryFilter model, CancellationToken cancellationToken)
    {
        return _userFacade.GetAllUsers(model, cancellationToken);
    }

}
