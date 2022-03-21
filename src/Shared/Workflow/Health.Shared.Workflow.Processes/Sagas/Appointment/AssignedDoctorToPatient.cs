using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Sagas.Appointment;

public interface AssignedDoctorToPatient
{
    Guid AppointmentId { get; }
    DoctorCardInformation Doctor { get; }
    
    DateTime Timestamp { get; }
}