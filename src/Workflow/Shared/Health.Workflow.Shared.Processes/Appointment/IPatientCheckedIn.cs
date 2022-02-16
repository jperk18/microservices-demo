namespace Health.Workflow.Shared.Processes.Appointment;

public interface IPatientCheckedIn
{
    Guid AppointmentId { get; }
    IPatientInformation Patient { get; }
    
    DateTime Timestamp { get; }
}

public interface IPatientInformation
{
    Guid Id { get; }
    string FirstName { get; }
    string LastName { get; }
}