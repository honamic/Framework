using Honamic.Framework.Commands;
using Honamic.IdentityPlus.Application.Accounts.Commands;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text;

namespace Honamic.IdentityPlus.Application.Accounts.CommandHandlers;
public class ValidateAccessTokenCommandHandler : ICommandHandler<ValidateAccessTokenCommand, bool>
{
    private readonly IOptionsMonitor<BearerTokenOptions> bearerTokenOptions;
    public ValidateAccessTokenCommandHandler(IOptionsMonitor<BearerTokenOptions> bearerTokenOptions)
    {
        this.bearerTokenOptions = bearerTokenOptions;

    }

    public Task<bool> HandleAsync(ValidateAccessTokenCommand command, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            //var claimsPrincipal = tokenHandler.ValidateToken(command.AccessToken, new TokenValidationParameters
            //{
            //    ValidIssuer = bearerTokenOptions.CurrentValue.ClaimsIssuer, // site that makes the token
            //    ValidateIssuer = true,
            //    ValidAudience = bearerTokenOptions.CurrentValue., // site that consumes the token
            //    ValidateAudience = true,
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key)),
            //    ValidateIssuerSigningKey = true, // verify signature to avoid tampering
            //    ValidateLifetime = true, // validate the expiration
            //    ClockSkew = TimeSpan.Zero // tolerance for the expiration date
            //}, out var securityToken);

            //var (success, message) =
            //    await IsValidClaimsPrincipalAsync(claimsPrincipal, securityToken, tokenType);
            //return (success, claimsPrincipal, message);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            return Task.FromResult(false);
        }
    }
}