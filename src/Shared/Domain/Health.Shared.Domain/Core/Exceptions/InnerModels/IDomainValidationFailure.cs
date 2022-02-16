namespace Health.Shared.Domain.Core.Exceptions.InnerModels;

public interface IDomainValidationFailure
{
    /// <summary>
    /// The name of the property.
    /// </summary>
    string? PropertyName { get; set; }

    /// <summary>
    /// The error message
    /// </summary>
    string ErrorMessage { get; }

    /// <summary>
    /// The property value that caused the failure.
    /// </summary>
    object? AttemptedValue { get; }
    
    /// <summary>
    /// Custom severity level associated with the failure.
    /// </summary>
    DomainSeverity Severity { get; }

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    string ErrorCode { get; }
}