namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface AppointmentScheduled
{
    Guid Appointment { get; }
}