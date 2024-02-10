using Honamic.Framework.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
using Honamic.IdentityPlus.Application.Accounts.Commands;

namespace Honamic.IdentityPlus.Application.Accounts.CommandHandlers;
public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, object?>
{
    private readonly SignInManager<User> signInManager;
    private readonly IOptionsMonitor<BearerTokenOptions> bearerTokenOptions;
    private readonly TimeProvider timeProvider;

    public RefreshTokenCommandHandler(SignInManager<User> signInManager,
        IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
        TimeProvider timeProvider)
    {
        this.signInManager = signInManager;
        this.bearerTokenOptions = bearerTokenOptions;
        this.timeProvider = timeProvider;
    }

    public async Task<object?> HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(command.RefreshToken);

        // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            timeProvider.GetUtcNow() >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not User user)
        {
            TypedResults.Challenge();

            return null;
        }
        var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);

        return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);

    }
}