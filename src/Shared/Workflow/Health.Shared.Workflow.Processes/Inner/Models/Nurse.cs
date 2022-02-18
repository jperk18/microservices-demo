namespace Health.Shared.Workflow.Processes.Inner.Models;

public interface Nurse : NurseIdentifier, NurseInfo
{
}

public interface NurseInfo
{
    string FirstName { get; }
    string LastName { get; }
    DateTime DateOfBirth { get; }
}

public interface NurseIdentifier
{
    Guid Id { get; }
}

public class NurseDto : Nurse
{
    public NurseDto(Guid id, string firstName, string lastName, DateTime dateOfBirth)
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