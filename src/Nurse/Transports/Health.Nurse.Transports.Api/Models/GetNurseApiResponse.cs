using Health.Nurse.Transports.Api.Models.Interfaces;

namespace Health.Nurse.Transports.Api.Models;

public class GetNurseApiResponse : INurse, INurseIdentifer
{
    public GetNurseApiResponse(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
}