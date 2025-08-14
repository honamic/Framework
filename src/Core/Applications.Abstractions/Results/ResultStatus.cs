namespace Honamic.Framework.Applications.Results;

public enum ResultStatus
{
    None = 0,
    [Obsolete("Use Success instead.", true)]
    Ok = Success,
    Success = 1,
    Unauthorized = 2,
    Unauthenticated = 3,
    UnhandledException = 4,
    ValidationError = 5,
    InvalidDomainState = 6,
    NotFound = 7,
}