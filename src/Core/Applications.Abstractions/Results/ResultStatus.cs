namespace Honamic.Framework.Applications.Results;

/// <summary>
/// Represents the outcome status of an operation.
/// </summary>
public enum ResultStatus
{
    /// <summary>
    /// Default value. No result has been set yet.
    /// </summary>
    None = 0,

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
    /// The operation failed without a specific reason.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// The user is authenticated but does not have permission to perform the operation.
    /// </summary>
    Unauthorized = 3,

    /// <summary>
    /// The user is not authenticated.
    /// </summary>
    Unauthenticated = 4,

    /// <summary>
    /// An unexpected exception occurred during the operation.
    /// </summary>
    UnhandledException = 5,

    /// <summary>
    /// One or more input values failed validation.
    /// </summary>
    ValidationError = 6,

    /// <summary>
    /// The domain entity is in an invalid state for the requested operation.
    /// </summary>
    InvalidDomainState = 7,

    /// <summary>
    /// The requested resource or entity was not found.
    /// </summary>
    NotFound = 8
}