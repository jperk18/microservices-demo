using Health.Patient.Domain.Console.Core.Models;
using Health.Shared.Domain.Mediator.Commands;

namespace Health.Patient.Domain.Console.Commands.CreatePatientCommand;

public sealed class CreatePatientCommand: ICommand<PatientRecord>
{
    public CreatePatientCommand(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}