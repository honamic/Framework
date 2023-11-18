namespace Honamic.Framework.Facade.Results;

public enum ResultStatus
{
    None = 0,
    Ok = 1,
    Unauthorized = 2,
    Unauthenticated = 3,
    UnhandledException = 4,
    ValidationError = 5,
    InvalidDomainState = 6,
    NotFound = 7,
}