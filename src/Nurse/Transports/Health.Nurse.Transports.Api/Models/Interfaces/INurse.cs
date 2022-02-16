namespace Health.Nurse.Transports.Api.Models.Interfaces;

public interface INurse
{
    string FirstName { get; }
    string LastName { get; }
    DateTime DateOfBirth { get; }
}