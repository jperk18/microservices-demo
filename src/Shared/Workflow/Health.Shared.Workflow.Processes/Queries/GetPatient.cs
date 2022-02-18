using Health.Shared.Workflow.Processes.Core.Exceptions.Models;
using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Queries;

public interface GetPatient
{
    public Guid Id { get; }
}

public interface GetPatientSuccess
{
    Patient Patient { get; }
}

public interface GetPatientFailed
{
    WorkflowValidation Error { get; }
}