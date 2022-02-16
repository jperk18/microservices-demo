using Health.Nurse.Domain.Console.Commands.Core;
using Health.Nurse.Domain.Console.Core.Models;

namespace Health.Nurse.Domain.Console.Commands.CreateNurseCommand;

public sealed class CreateNurseCommand: ICommand<NurseRecord>
{
    public CreateNurseCommand(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}