using Honamic.Framework.Queries;

namespace Honamic.IdentityPlus.Application.Users.Queries;

public class GetAllUsersQueryResult : IQueryResult
{
    public long Id { get; set; }

    public string? Username { get; set; } 

    public string? Email { get; set; } 

    public string? PhoneNumber { get; set; } 
}
