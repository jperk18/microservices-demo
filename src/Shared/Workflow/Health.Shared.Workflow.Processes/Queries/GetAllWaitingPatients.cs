
namespace Health.Shared.Workflow.Processes.Queries;

public interface GetAllWaitingPatients
{
}

public interface GetAllWaitingPatientsSuccess
{
    Guid[] Patients { get; }
}