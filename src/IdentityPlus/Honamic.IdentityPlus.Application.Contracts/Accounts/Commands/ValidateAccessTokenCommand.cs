using Honamic.Framework.Commands;

namespace Honamic.IdentityPlus.Application.Accounts.Commands;
public class ValidateAccessTokenCommand : ICommand<bool>
{
    public string AccessToken { get; set; } = default!;
}
