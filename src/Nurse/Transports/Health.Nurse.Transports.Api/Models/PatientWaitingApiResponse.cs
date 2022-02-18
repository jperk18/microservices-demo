namespace Health.Nurse.Transports.Api.Models;

public class PatientWaitingApiResponse
{
    public PatientWaitingApiResponse(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}