namespace Health.Workflow.Processes.Core.Exceptions.Models;

public interface IWorkflowValidation
{
    IEnumerable<ValidationFailure>? Errors { get; set; }
}