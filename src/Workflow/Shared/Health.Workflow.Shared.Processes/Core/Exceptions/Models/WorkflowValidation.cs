namespace Health.Workflow.Shared.Processes.Core.Exceptions.Models;

public class WorkflowValidation : IWorkflowValidation
{
    public WorkflowValidation()
    {
    }
    public WorkflowValidation(string message)
    {
        Message = message;
    }

    public IEnumerable<WorkflowValidationFailure>? Errors { get; set; }
    public string Message { get; set; }
    
    public WorkflowValidationException ToException()
    {
        return new WorkflowValidationException(Message, Errors);
    }
}