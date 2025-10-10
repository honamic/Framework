namespace Honamic.Framework.Application.Results;

/// <summary>
/// Represents the outcome status of an operation.
/// </summary>
public enum ResultStatus
{
    /// <summary>
    /// Default value. No result has been set yet.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Obsolete. Use <see cref="Success"/> instead.
    /// </summary>
    [Obsolete("Use Success instead.", true)]
    Ok = Success,
    /// <summary>
    /// The operation completed successfully.
    /// </summary>
    Success = 1,

    /// <summary>
    /// The user is not authenticated.
    /// </summary>
    AuthenticationRequired = 2,

    /// <summary>
    /// The user is authenticated but does not have permission to perform the operation.
    /// </summary>
    Forbidden = 3,

    /// <summary>
    /// An unexpected exception occurred during the operation.
    /// </summary>
    UnhandledException = 4,

    /// <summary>
    /// One or more input values failed validation.
    /// </summary>
    ValidationFailed = 5,

    /// <summary>
    /// The domain entity is in an invalid state for the requested operation.
    /// </summary>
    DomainStateInvalid = 6,

    /// <summary>
    /// The requested resource or entity was not found.
    /// </summary>
    NotFound = 7
}