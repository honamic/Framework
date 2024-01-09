using Honamic.Framework.Commands;

namespace Honamic.IdentityPlus.Application.Users.Commands;
public class RefreshTokenCommand : ICommand<object?>
{
    public string RefreshToken { get; set; } = default!;
}
