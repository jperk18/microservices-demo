namespace Health.Shared.Workflow.Processes.Core.Exceptions.Models;

public interface WorkflowValidation
{
    string Message { get; }
    WorkflowValidationFailure[]? Errors { get; }
}

public class WorkflowValidationDto : WorkflowValidation
{
    
    public WorkflowValidationDto()
    {
    }
    
    public WorkflowValidationDto(WorkflowValidation wm)
    {
        Message = wm.Message;
        Errors = wm.Errors;
    }
    
    public WorkflowValidationDto(string message)
    {
        Message = message;
    }

    public WorkflowValidationFailure[]? Errors { get; set; }
    public string Message { get; set; } = null!;

    public WorkflowValidationException ToException()
    {
        return new WorkflowValidationException(this);
    }
}