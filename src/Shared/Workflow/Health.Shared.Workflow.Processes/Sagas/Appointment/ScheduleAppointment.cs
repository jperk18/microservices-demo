namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface ScheduleAppointment
{
    Guid AppointmentId { get; }
    Guid PatientId { get; }

}