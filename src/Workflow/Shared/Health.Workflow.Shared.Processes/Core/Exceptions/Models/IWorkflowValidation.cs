namespace Health.Workflow.Shared.Processes.Core.Exceptions.Models;

public interface IWorkflowValidation
{
    IEnumerable<WorkflowValidationFailure>? Errors { get; set; }
}