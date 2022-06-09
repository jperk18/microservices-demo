namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface PatientCheckedIn
{
    Guid AppointmentId { get; }
}