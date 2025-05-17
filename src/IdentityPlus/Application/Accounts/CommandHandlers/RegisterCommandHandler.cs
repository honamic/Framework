using Honamic.Framework.Commands;
using Honamic.Framework.Domain;
using Honamic.IdentityPlus.Application.Accounts.Commands.Register;
using Honamic.IdentityPlus.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Honamic.IdentityPlus.Application.Accounts.CommandHandlers;
internal class RegisterCommandHandler(UserManager<User> userManager,
    IIdGenerator idGenerator)
    : ICommandHandler<RegisterCommand, RegisterCommandResult>
{
    public async Task<RegisterCommandResult> HandleAsync(RegisterCommand command, CancellationToken cancellationToken)
    {
        User user = new User { UserName = command.UserName, Email = command.Email };
        user.SetManualId(idGenerator.GetNewId());
        IdentityResult result = await userManager.CreateAsync(user, command.Password);

        return new RegisterCommandResult
        {
            Succeeded = result.Succeeded,
            Errors = result.Errors.Select(q => q.Description).ToList()
        };
    }
}
