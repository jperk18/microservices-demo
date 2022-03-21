using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface RecordedPatientVitals
{
    Guid AppointmentId { get; }
    PatientIdentifier PatientId { get; }
    
    string Vitals { get; }
    DateTime Timestamp { get; }
}