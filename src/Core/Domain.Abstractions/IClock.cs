namespace Honamic.Framework.Domain;

public interface IClock
{
    DateTime Now { get; }

    DateTime UtcNow { get; }

    DateTimeOffset NowWithOffset { get; }
}
