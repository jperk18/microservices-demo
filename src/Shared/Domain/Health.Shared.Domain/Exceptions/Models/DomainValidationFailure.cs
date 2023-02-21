namespace Health.Shared.Domain.Exceptions.Models;

public interface DomainValidationFailure
{
    /// <summary>
    /// The name of the property.
    /// </summary>
    string? PropertyName { get; }

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


public class DomainValidationFailureDto : DomainValidationFailure
{
    public DomainValidationFailureDto(string errorCode, string errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public string? PropertyName { get; set; }
    public string ErrorMessage { get; set; }
    public object? AttemptedValue { get; set; }
    public DomainSeverity Severity { get; set; }
    public string ErrorCode { get; set; }
}