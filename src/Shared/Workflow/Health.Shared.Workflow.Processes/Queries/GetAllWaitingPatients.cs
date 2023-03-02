using Health.Shared.Workflow.Processes.Exceptions.Models;

namespace Health.Shared.Workflow.Processes.Queries;

public interface GetAllWaitingPatients
{
}

public interface GetAllWaitingPatientsSuccess
{
    Guid[] Patients { get; }
}

public interface GetAllWaitingPatientsFailed
{
    WorkflowValidation Error { get; }
}