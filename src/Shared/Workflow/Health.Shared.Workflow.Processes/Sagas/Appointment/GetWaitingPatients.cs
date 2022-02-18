using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface GetWaitingPatientsForNurses
{
}

public interface WaitingPatientsForNurses
{
    PatientBasicInformation PatientInformation { get; }
}