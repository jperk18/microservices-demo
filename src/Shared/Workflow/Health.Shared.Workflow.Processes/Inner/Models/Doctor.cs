namespace Health.Shared.Workflow.Processes.Inner.Models;

public interface Doctor : DoctorIdentifier, DoctorInfo, DoctorCardInformation
{
}

public interface DoctorCardInformation : DoctorIdentifier, DoctorBasicInformation
{
}

public interface DoctorBasicInformation
{
    string FirstName { get; }
    string LastName { get; }
}

public interface DoctorInfo : NurseBasicInformation
{
    DateTime DateOfBirth { get; }
}

public interface DoctorIdentifier
{
    Guid Id { get; }
}

public class DoctorDto : Nurse
{
    public DoctorDto(Guid id, string firstName, string lastName, DateTime dateOfBirth)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
}