namespace Health.Shared.Workflow.Processes.Exceptions.Models;

public interface WorkflowValidation
{
    OriginatingService Origination { get; }
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
        Origination = wm.Origination;
        Message = wm.Message;
        Errors = wm.Errors;
    }
    
    public WorkflowValidationDto(OriginatingService origination, string message)
    {
        Origination = origination;
        Message = message;
    }

    public WorkflowValidationFailure[]? Errors { get; set; }
    public OriginatingService Origination { get; } = OriginatingService.Unknown!;
    public string Message { get; set; } = null!;

    public WorkflowValidationException ToException()
    {
        return new WorkflowValidationException(this);
    }
}