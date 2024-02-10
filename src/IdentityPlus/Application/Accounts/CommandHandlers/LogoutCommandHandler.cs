using Honamic.Framework.Commands;
using Honamic.IdentityPlus.Application.Accounts.Commands;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Honamic.IdentityPlus.Application.Accounts.CommandHandlers;
public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly SignInManager<User> signInManager;

    public LogoutCommandHandler(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }

    public Task HandleAsync(LogoutCommand command, CancellationToken cancellationToken)
    {
        return signInManager.SignOutAsync();
    }
}