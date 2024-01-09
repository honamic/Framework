using Honamic.Framework.Commands;
using Honamic.IdentityPlus.Application.Users.Commands;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Honamic.IdentityPlus.Application.Users.CommandHandlers;
internal class LoginCommandHandler : ICommandHandler<LoginCommand, LoginCommandResult>
{
    private readonly SignInManager<User> signInManager;

    public LoginCommandHandler(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }

    public async Task<LoginCommandResult> HandleAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        var useCookieScheme = (command.UseCookies == true) || (command.UseSessionCookies == true);
        var isPersistent = (command.UseCookies == true) && (command.UseSessionCookies != true);
        
        signInManager.AuthenticationScheme = useCookieScheme
            ? IdentityConstants.ApplicationScheme
            : IdentityConstants.BearerScheme;
        
        var result = await signInManager
                .PasswordSignInAsync(command.UserName, command.Password, isPersistent, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(command.TwoFactorCode))
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(command.TwoFactorCode, isPersistent, rememberClient: isPersistent);
            }
            else if (!string.IsNullOrEmpty(command.TwoFactorRecoveryCode))
            {
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(command.TwoFactorRecoveryCode);
            }
        }

        return new LoginCommandResult
        {
            IsLockedOut = result.IsLockedOut,
            IsNotAllowed = result.IsNotAllowed,
            RequiresTwoFactor = result.RequiresTwoFactor,
            Succeeded = result.Succeeded
        };
    }

}
