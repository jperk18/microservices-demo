using Health.Shared.Workflow.Processes.Core.Exceptions.Models;
using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Queries;

public interface GetNurse
{
    public Guid Id { get; set; }
}

public interface GetNurseSuccess
{
    Nurse Nurse { get; }
}

public interface GetNurseFailed
{
    WorkflowValidation Error { get; }
}