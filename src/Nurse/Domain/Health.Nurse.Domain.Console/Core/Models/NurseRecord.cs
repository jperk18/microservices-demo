using Health.Nurse.Domain.Console.Core.Models.Interfaces;

namespace Health.Nurse.Domain.Console.Core.Models;

public class NurseRecord : INurseRecord
{
    public NurseRecord(string firstName, string lastName, DateTime dateOfBirth, Guid patientId)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Id = patientId;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Guid Id { get; set; }
}