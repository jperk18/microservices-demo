namespace Health.Shared.Workflow.Processes.Inner.Models;
public interface PatientCardInformation : PatientIdentifier, PatientBasicInformation
{
}

public interface PatientIdentifier
{
    Guid Id { get; }
}

public interface PatientBasicInformation
{
    string FirstName { get; }
    string LastName { get; }
}

public interface PatientInfo : PatientBasicInformation
{
    DateTime DateOfBirth { get; }
}

public interface Patient : PatientInfo, PatientCardInformation
{
}

public class PatientDto : Patient
{
    public PatientDto(Guid patientId, string firstName, string lastName, DateTime dateOfBirth)
    {
        Id = patientId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
}