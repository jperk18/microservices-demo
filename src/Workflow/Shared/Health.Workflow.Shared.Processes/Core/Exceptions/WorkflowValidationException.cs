using Health.Workflow.Shared.Processes.Core.Exceptions.Models;

namespace Health.Workflow.Shared.Processes.Core.Exceptions;

public class WorkflowValidationException : Exception, IWorkflowValidation
{
    public WorkflowValidationException(string message)
    {
        Message = message;
        Errors = new List<WorkflowValidationFailure>();
    }

    public WorkflowValidationException(string message, IEnumerable<WorkflowValidationFailure>? errors)
    {
        Message = message;
        Errors = errors;
    }
    
    public WorkflowValidation ToValidation()
    {
        return new WorkflowValidation(Message)
        {
            Errors = Errors
        };
    }

    public IEnumerable<WorkflowValidationFailure>? Errors { get; set; }
    public override string Message { get; }
}