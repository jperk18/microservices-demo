using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface AssignedNurseToPatient
{
    Guid AppointmentId { get; }
    NurseCardInformation Nurse { get; }
    
    DateTime Timestamp { get; }
}