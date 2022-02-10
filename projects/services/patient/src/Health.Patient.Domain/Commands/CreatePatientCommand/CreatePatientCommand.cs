using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Core.Models;

namespace Health.Patient.Domain.Commands.CreatePatientCommand;

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