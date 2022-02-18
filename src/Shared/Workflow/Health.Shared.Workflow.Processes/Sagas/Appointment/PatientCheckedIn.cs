using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface PatientCheckedIn
{
    Guid AppointmentId { get; }
    PatientBasicInformation Patient { get; }
    
    DateTime Timestamp { get; }
}