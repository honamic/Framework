using Honamic.Framework.Events;

namespace Honamic.Framework.Domain.Defaults;

public class DomainEventUserInfo : IEventUserInfo
{
    public long? UserId { get; set; }
    public string? UserFullName { get; set; }

    public DomainEventUserInfo()
    {

    }

    public DomainEventUserInfo(long userId)
    {
        UserId = userId;
    }

    public DomainEventUserInfo(long userId, string userFullName)
    {
        UserId = userId;
        UserFullName = userFullName;
    }
}