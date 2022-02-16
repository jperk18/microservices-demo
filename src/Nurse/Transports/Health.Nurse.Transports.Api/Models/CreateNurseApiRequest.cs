using Health.Nurse.Transports.Api.Models.Interfaces;

namespace Health.Nurse.Transports.Api.Models;

public class CreateNurseApiRequest : INurse
{
    public CreateNurseApiRequest(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
}