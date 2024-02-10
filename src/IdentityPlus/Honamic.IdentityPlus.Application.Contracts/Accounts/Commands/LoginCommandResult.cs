namespace Honamic.IdentityPlus.Application.Accounts.Commands;

public class LoginCommandResult
{
    public bool Succeeded { get; init; }

    /// <summary>
    /// Returns a flag indication whether the user attempting to sign-in is locked out.
    /// </summary>
    /// <value>True if the user attempting to sign-in is locked out, otherwise false.</value>
    public bool IsLockedOut { get; init; }

    /// <summary>
    /// Returns a flag indication whether the user attempting to sign-in is not allowed to sign-in.
    /// </summary>
    /// <value>True if the user attempting to sign-in is not allowed to sign-in, otherwise false.</value>
    public bool IsNotAllowed { get; init; }

    /// <summary>
    /// Returns a flag indication whether the user attempting to sign-in requires two factor authentication.
    /// </summary>
    /// <value>True if the user attempting to sign-in requires two factor authentication, otherwise false.</value>
    public bool RequiresTwoFactor { get; init; }


    public override string ToString()
    {
        return IsLockedOut ? "LockedOut" :
               IsNotAllowed ? "NotAllowed" :
               RequiresTwoFactor ? "RequiresTwoFactor" :
               Succeeded ? "Succeeded" : "Failed";
    }
}