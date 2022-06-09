namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface AppointmentScheduled
{
    Guid AppointmentId { get; }
    Guid PatientId { get; }

}