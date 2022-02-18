namespace Health.Shared.Workflow.Processes.Core.Exceptions.Models;

public interface WorkflowValidationFailure
{
    /// <summary>
    /// The name of the property.
    /// </summary>
    string? PropertyName { get; set; }

    /// <summary>
    /// The error message
    /// </summary>
    string ErrorMessage { get; set; }

    /// <summary>
    /// The property value that caused the failure.
    /// </summary>
    object? AttemptedValue { get; set; }

    /// <summary>
    /// Custom severity level associated with the failure.
    /// </summary>
    WorkflowSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    string ErrorCode { get; set; }
}

public class WorkflowValidationFailureDto : WorkflowValidationFailure
{
    private string _genericErrorMessage = "";

    public WorkflowValidationFailureDto()
    {
        ErrorCode = _genericErrorMessage;
        ErrorMessage = _genericErrorMessage;
    }
    public WorkflowValidationFailureDto(string errorCode)
    {
        ErrorCode = errorCode;
        ErrorMessage = _genericErrorMessage;
    }
    
    public WorkflowValidationFailureDto(string errorCode, string errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
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
    public WorkflowSeverity Severity { get; set; } = WorkflowSeverity.Error;

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string ErrorCode { get; set; }
}