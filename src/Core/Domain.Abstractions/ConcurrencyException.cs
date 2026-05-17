namespace Honamic.Framework.Domain;

public class ConcurrencyException : BusinessException
{
    public long CurrentVersion { get; }
    public long ClientVersion { get; }

    public ConcurrencyException(long currentVersion, long clientVersion)
        : base(
            message: $"This information has changed. Please refresh the data and try again.",
            code: "CONCURRENCY_CONFLICT")
    {
        CurrentVersion = currentVersion;
        ClientVersion = clientVersion;
    }
}