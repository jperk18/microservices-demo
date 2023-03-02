using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Queries;

public interface GetAllPatients
{
}

public interface GetAllPatientsSuccess
{
    Patient[] Patients { get; }
}