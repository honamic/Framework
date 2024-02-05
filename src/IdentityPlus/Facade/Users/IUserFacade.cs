using Honamic.Framework.Facade;
using Honamic.Framework.Facade.Results;
using Honamic.IdentityPlus.Application.Users.CommandHandlers;
using Honamic.IdentityPlus.Application.Users.Commands;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Honamic.IdentityPlus.Facade.Users;

public interface IUserFacade : IBaseFacade
{
    Task<Result> Login(LoginCommand command, CancellationToken cancellationToken);
    Task<Result> Logout(LogoutCommand command, CancellationToken cancellationToken);
    Task<Result<SignInHttpResult?>> RefrehToken(RefreshTokenCommand command, CancellationToken cancellationToken);
    Task ValidateAccessToken(ValidateAccessTokenCommand command, CancellationToken cancellationToken);
}