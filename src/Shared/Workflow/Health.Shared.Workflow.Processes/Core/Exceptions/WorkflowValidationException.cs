using Health.Shared.Workflow.Processes.Core.Exceptions.Models;

namespace Health.Shared.Workflow.Processes.Core.Exceptions;

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
        return new WorkflowValidationDto(Message)
        {
            Errors = Errors
        };
    }

    public WorkflowValidationFailure[] Errors { get; set; } = null!;
    public override string Message { get; } = null!;
}