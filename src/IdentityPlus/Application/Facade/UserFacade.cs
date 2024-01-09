using Honamic.Framework.Commands;
using Honamic.Framework.Facade;
using Honamic.Framework.Facade.Results;
using Honamic.IdentityPlus.Application.Users.CommandHandlers;
using Honamic.IdentityPlus.Application.Users.Commands;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace Honamic.IdentityPlus.Application.Facade;
public class UserFacade : BaseFacade, IUserFacade
{
    private readonly ICommandBus commandBus;

    public UserFacade(ILogger<UserFacade> logger, ICommandBus commandBus) : base(logger)
    {
        this.commandBus = commandBus;
    }

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
}
