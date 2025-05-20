using Honamic.Framework.Applications.Results;
using Honamic.Framework.Facade;
using Honamic.IdentityPlus.Application.Accounts.Commands;
using Honamic.IdentityPlus.Application.Accounts.Commands.Register;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Honamic.IdentityPlus.Facade.Accounts;

public interface IAccountFacade : IBaseFacade
{
    Task<Result> Register(RegisterCommand command, CancellationToken cancellationToken);
    Task<Result> Login(LoginCommand command, CancellationToken cancellationToken);
    Task<Result> Logout(LogoutCommand command, CancellationToken cancellationToken);
    Task<Result<SignInHttpResult?>> RefrehToken(RefreshTokenCommand command, CancellationToken cancellationToken);
    Task ValidateAccessToken(ValidateAccessTokenCommand command, CancellationToken cancellationToken);
}