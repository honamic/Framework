using Honamic.Framework.Commands;
using Honamic.Framework.Facade;
using Honamic.Framework.Facade.Results;
using Honamic.Framework.Queries;
using Honamic.IdentityPlus.Application.Users.CommandHandlers;
using Honamic.IdentityPlus.Application.Users.Commands;
using Honamic.IdentityPlus.Application.Users.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace Honamic.IdentityPlus.Facade.Users;
public class UserFacade(ILogger<UserFacade> logger, ICommandBus commandBus, IQueryBus queryBus) : BaseFacade(logger), IUserFacade
{
    public async Task<Result> Login(LoginCommand model, CancellationToken cancellationToken)
    {
        var commandResult = await commandBus.DispatchAsync<LoginCommand, LoginCommandResult>(model, cancellationToken);

        if (!commandResult.Succeeded)
        {
            var result = new Result(ResultStatus.Unauthorized);
            result.AppendError(commandResult.ToString());
            return result;
        }

        return ResultStatus.Ok;
    }

    public async Task<Result> Logout(LogoutCommand command, CancellationToken cancellationToken)
    {
        await commandBus.DispatchAsync<LogoutCommand>(command, cancellationToken);

        return ResultStatus.Ok;
    }

    public async Task<Result<SignInHttpResult?>> RefrehToken(RefreshTokenCommand model, CancellationToken cancellationToken)
    {
        var commandResult = await commandBus.DispatchAsync<RefreshTokenCommand, object>(model, cancellationToken);

        if (commandResult is SignInHttpResult signInHttpResult)
            return signInHttpResult;

        var result = new Result<SignInHttpResult?>();

        result.Status = ResultStatus.Unauthorized;

        return result;
    }

    public Task ValidateAccessToken(ValidateAccessTokenCommand model, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    public async Task<Result<PagedQueryResult<GetAllUsersQueryResult>>> GetAllUsers(GetAllUsersQueryFilter filter, CancellationToken cancellationToken)
    {
        var result = await queryBus.Dispatch<GetAllUsersQueryFilter, PagedQueryResult<GetAllUsersQueryResult>>(filter, cancellationToken);

        return result;
    }

}
