namespace Honamic.IdentityPlus.Application.Users.Commands;


public class BererTokenResult
{
    public BererTokenResult()
    {
        
    }

    // not in Identity
    public DateTimeOffset? CreateOn { get; set; }

    public string TokenType { get; }

    public required string AccessToken { get; init; }

    public required long ExpiresIn { get; init; }
  
    public required string RefreshToken { get; init; }
}