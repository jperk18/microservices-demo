using Health.Patient.Transports.Api.Models.Interfaces;

namespace Health.Patient.Transports.Api.Models;

public class CreatePatientApiRequest : IPatient
{
    public CreatePatientApiRequest(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public DateTime DateOfBirth { get; }
}