namespace Honamic.Framework.Domain;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTimeOffset NowWithOffset => DateTimeOffset.Now;

    public DateTime Now => DateTime.Now;
}