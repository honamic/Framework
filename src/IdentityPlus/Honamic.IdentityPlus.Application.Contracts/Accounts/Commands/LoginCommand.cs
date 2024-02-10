using Honamic.Framework.Commands;

namespace Honamic.IdentityPlus.Application.Accounts.Commands;
public class LoginCommand : ICommand<LoginCommandResult>
{
    public required string UserName { get; set; }

    public required string Password { get; set; }
    //
    // Summary:
    //     The optional two-factor authenticator code. This may be required for users who
    //     have enabled two-factor authentication. This is not required if a Microsoft.AspNetCore.Identity.Data.LoginRequest.TwoFactorRecoveryCode
    //     is sent.
    public string? TwoFactorCode { get; set; }
    //
    // Summary:
    //     An optional two-factor recovery code from Microsoft.AspNetCore.Identity.Data.TwoFactorResponse.RecoveryCodes.
    //     This is required for users who have enabled two-factor authentication but lost
    //     access to their Microsoft.AspNetCore.Identity.Data.LoginRequest.TwoFactorCode.
    public string? TwoFactorRecoveryCode { get; set; }


    public bool? UseCookies { get; set; }

    public bool? UseSessionCookies { get; set; }
}
