using Health.Shared.Workflow.Processes.Exceptions.Models;

namespace Health.Shared.Workflow.Processes.Exceptions;

public class WorkflowValidationException : Exception, WorkflowValidation
{
    public WorkflowValidationException()
    {
    }

    public WorkflowValidationException(WorkflowValidation wm)
    {
        Message = wm.Message;
        Errors = wm.Errors ?? (new WorkflowValidationFailureDto[]{}).ToArray<WorkflowValidationFailure>();
    }
    
    public WorkflowValidationException(string message)
    {
        Message = message;
        Errors = (new WorkflowValidationFailureDto[]{}).ToArray<WorkflowValidationFailure>();
    }

    public WorkflowValidationDto ToValidation()
    {
        return new WorkflowValidationDto(Origination, Message)
        {
            Errors = Errors
        };
    }

    public WorkflowValidationFailure[] Errors { get; set; } = null!;
    public OriginatingService Origination { get; } = OriginatingService.Unknown!;
    public override string Message { get; } = null!;
}