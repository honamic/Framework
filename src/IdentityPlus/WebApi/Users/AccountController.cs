using Honamic.Framework.Applications.Results;
using Honamic.Framework.Facade.Web.Results;
using Honamic.IdentityPlus.Application.Accounts.Commands;
using Honamic.IdentityPlus.Application.Accounts.Commands.Register;
using Honamic.IdentityPlus.Application.Accounts.Queries;
using Honamic.IdentityPlus.Facade.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Honamic.IdentityPlus.WebApi.Users;


[Route("api/Account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountFacade _userFacade;

    public AccountController(IAccountFacade userFacade)
    {
        _userFacade = userFacade;
    }


    [HttpPost("Register")]
    public async Task<ActionResult<Result>?> Register([FromBody] RegisterCommand model, CancellationToken cancellationToken)
    {
        var result = await _userFacade.Register(model, cancellationToken);

        if (result.Status == ResultStatus.Unauthorized)
        {
            return this.ResultToAction(result);
        }

        return Empty;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<Result>?> Login([FromBody] LoginCommand model, CancellationToken cancellationToken)
    {
        var result = await _userFacade.Login(model, cancellationToken);

        if (result.Status == ResultStatus.Unauthorized)
        {
            return this.ResultToAction(result);
        }

        return Empty;
    }

    [HttpPost("Logout")]
    [Authorize]
    public async Task<bool> Logout([FromBody] LogoutCommand model, CancellationToken cancellationToken)
    {
        if (User?.Identity?.IsAuthenticated == false)
        {
            return true;
        }

        var result = await _userFacade.Logout(model, cancellationToken);

        return result.Status == ResultStatus.Ok;
    }


    [HttpPost("RefreshToken")]
    public async Task<IResult> RefreshToken([FromBody] RefreshTokenCommand model, CancellationToken cancellationToken)
    {
        var result = await _userFacade.RefrehToken(model, cancellationToken);

        if (result.Status == ResultStatus.Unauthorized || result.Data is null)
        {
            return Results.StatusCode(401);
        }

        return result.Data;
    }

    [HttpPost("ValidateAccessToken")]
    [Authorize]
    public Task<bool?> ValidateAccessToken(CancellationToken cancellationToken)
    {
        return Task.FromResult(User.Identity?.IsAuthenticated);
    }

    [HttpGet("MyClientClaims")]
    [Authorize]
    public Task<List<ClaimValue>> GetMyClientClaims(CancellationToken cancellationToken)
    {
        var allowClaimTypes = new[] {
            ClaimTypes.NameIdentifier,
            ClaimTypes.Email,
            ClaimTypes.Name,
        };
        var list = User.Claims.Where(c => allowClaimTypes.Any(d => d.Equals(c.Type))).ToList();
        var result = list.Select(c => new ClaimValue
        {
            Type = c.Type,
            Value = c.Value
        }).ToList();

        return Task.FromResult(result);
    }
}
