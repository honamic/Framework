namespace Honamic.Framework.Domain;

public abstract class ValueObject : IEquatable<ValueObject?>
{
    protected abstract IEnumerable<object> GetEqualityComponents();
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public bool Equals(ValueObject? other) => Equals(other as object);

    public override int GetHashCode() => HashCode.Combine(GetEqualityComponents);
}