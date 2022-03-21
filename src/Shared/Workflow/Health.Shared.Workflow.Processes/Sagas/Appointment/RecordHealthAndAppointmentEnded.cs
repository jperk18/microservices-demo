using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface RecordHealthAndAppointmentEnded
{
    Guid AppointmentId { get; }
    DateTime Timestamp { get; }
}