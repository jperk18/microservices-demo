using Health.Workflow.Processes.Core.Exceptions.Models;

namespace Health.Workflow.Processes.Core.Exceptions;

public class WorkflowValidationException : Exception, IWorkflowValidation
{
    public WorkflowValidationException(string message)
    {
        Message = message;
        Errors = new List<ValidationFailure>();
    }

    public WorkflowValidationException(string message, IEnumerable<ValidationFailure>? errors)
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

    public IEnumerable<ValidationFailure>? Errors { get; set; }
    public override string Message { get; }
}