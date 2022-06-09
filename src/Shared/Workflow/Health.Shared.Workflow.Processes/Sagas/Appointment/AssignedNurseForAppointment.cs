namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface AssignedNurseForAppointment
{
    Guid AppointmentId { get; }
    Guid NurseId { get; }
}