using Honamic.Framework.Domain;
using System.Reflection;

namespace Honamic.IdentityPlus.Domain.Users;
public partial class User : AggregateRoot<long>, IEquatable<long> 
{
    public User()
    {
        RaiseEvent(new UserCreatedEvent(Id));
    }

    public User(string userName) : this()
    {
        UserName = userName;
    }

    public void SetManualId(long id)
    {
        Id = id;
        var createdEvent = this.Events.OfType<UserCreatedEvent>().FirstOrDefault();
        if (createdEvent != null)
        {
            Type type = createdEvent.GetType();
            PropertyInfo? prop = type.BaseType?.GetProperty(nameof(createdEvent.AggregateId));
            prop?.SetValue(createdEvent, id);
        }
    }

    public bool Equals(long other)
    {
        return Id == other;
    }
}