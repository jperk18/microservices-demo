using Health.Workflow.Shared.Processes.Appointment;

namespace Health.Workflow.Shared.Processes;

public interface GetWaitingPatientsForNurses
{
}

public interface WaitingPatientsForNurses
{
    IPatientInformation PatientInformation { get; }
}