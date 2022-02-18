namespace Health.Shared.Workflow.Processes.Inner.Models;

public interface Patient : PatientIdentifier, PatientInfo
{
}

public interface PatientBasicInformation : PatientIdentifier
{
    string FirstName { get; }
    string LastName { get; }
}

public interface PatientInfo
{
    string FirstName { get; }
    string LastName { get; }
    DateTime DateOfBirth { get; }
}

public interface PatientIdentifier
{
    Guid PatientId { get; }
}

public class PatientDto : Patient
{
    public PatientDto(Guid patientId, string firstName, string lastName, DateTime dateOfBirth)
    {
        PatientId = patientId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public Guid PatientId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
}