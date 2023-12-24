using Honamic.Framework.Domain;

namespace Honamic.IdentityPlus.Domain.Users;
public class UserCreatedEvent : DomainEvent
{
    public UserCreatedEvent(long userId) : base(userId)
    {

    }
}
