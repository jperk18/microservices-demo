namespace Health.Nurse.Domain.Console.Core.Exceptions.Models;

public class DomainValidationFailure
{
    public DomainValidationFailure()
    {
    }
    public DomainValidationFailure(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// The name of the property.
    /// </summary>
    public string? PropertyName { get; set; }

    /// <summary>
    /// The error message
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// The property value that caused the failure.
    /// </summary>
    public object? AttemptedValue { get; set; }
    
    /// <summary>
    /// Custom severity level associated with the failure.
    /// </summary>
    public DomainSeverity Severity { get; set; } = DomainSeverity.Error;

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string ErrorCode { get; set; }
}