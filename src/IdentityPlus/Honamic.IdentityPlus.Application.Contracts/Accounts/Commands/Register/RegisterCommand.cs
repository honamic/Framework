using Honamic.Framework.Commands;

namespace Honamic.IdentityPlus.Application.Accounts.Commands.Register;
public class RegisterCommand : ICommand<RegisterCommandResult>
{
    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }
}
