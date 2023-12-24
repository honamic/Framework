using Honamic.Framework.Events;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Honamic.IdentityPlus.Application;
public class IdentityPlusSignInManager : SignInManager<User>
{
    private readonly IEventBus _eventBus;

    public IdentityPlusSignInManager(UserManager<User> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<User> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<User>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<User> confirmation,
        IEventBus eventBus
        )
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
        _eventBus = eventBus;
    }

    public override async Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var result = await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);

        await PublishLoggedEvent(user, result);

        return result;
    }


    protected override async Task<SignInResult> SignInOrTwoFactorAsync(User user, bool isPersistent, string? loginProvider = null, bool bypassTwoFactor = false)
    {
        var result = await base.SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);

        await PublishLoggedEvent(user, result);

        return result;
    }

    private async Task PublishLoggedEvent(User user, SignInResult result)
    {
        if (result.Succeeded)
        {
            await _eventBus.PublishAsync(new UserLoggedEvent(user.Id, user.UserName));
        }
    }

}
