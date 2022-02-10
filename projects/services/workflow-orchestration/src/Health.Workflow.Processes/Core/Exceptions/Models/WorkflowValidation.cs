namespace Health.Workflow.Processes.Core.Exceptions.Models;

public class WorkflowValidation : IWorkflowValidation
{
    public WorkflowValidation(string message)
    {
        Message = message;
    }

    public IEnumerable<ValidationFailure>? Errors { get; set; }
    public string Message { get; set; }
    
    public WorkflowValidationException ToException()
    {
        return new WorkflowValidationException(Message, Errors);
    }
}