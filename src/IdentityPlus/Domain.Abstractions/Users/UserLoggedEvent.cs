using Honamic.Framework.Domain;

namespace Honamic.IdentityPlus.Domain.Users;
public class UserLoggedEvent : DomainEvent
{
    public UserLoggedEvent(long userId, string? userName) : base(userId)
    {
        UserId = userId;
        UserName = userName;
    }

    public long UserId { get; }
    public string? UserName { get; }
}
