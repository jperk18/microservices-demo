namespace Health.Patient.Domain.Console.Core.Models.Interfaces;

public interface IPatient
{
    string FirstName { get; set; }
    string LastName { get; set; }
    DateTime DateOfBirth { get; set; }
}