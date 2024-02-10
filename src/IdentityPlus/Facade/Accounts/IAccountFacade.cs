using Honamic.Framework.Facade;
using Honamic.Framework.Facade.Results;
using Honamic.Framework.Queries;
using Honamic.IdentityPlus.Application.Accounts.Commands;
using Honamic.IdentityPlus.Application.Users.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Honamic.IdentityPlus.Facade.Accounts;

public interface IAccountFacade : IBaseFacade
{
    Task<Result> Login(LoginCommand command, CancellationToken cancellationToken);
    Task<Result> Logout(LogoutCommand command, CancellationToken cancellationToken);
    Task<Result<SignInHttpResult?>> RefrehToken(RefreshTokenCommand command, CancellationToken cancellationToken);
    Task ValidateAccessToken(ValidateAccessTokenCommand command, CancellationToken cancellationToken);
}