using Honamic.Framework.Commands;

namespace Honamic.IdentityPlus.Application.Users.CommandHandlers;
public class ValidateAccessTokenCommand : ICommand<bool>
{
    public string AccessToken { get; set; } = default!;
}
