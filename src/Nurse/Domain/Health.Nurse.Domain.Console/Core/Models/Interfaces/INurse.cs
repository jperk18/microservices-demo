namespace Health.Nurse.Domain.Console.Core.Models.Interfaces;

public interface INurse
{
    string FirstName { get; set; }
    string LastName { get; set; }
    DateTime DateOfBirth { get; set; }
}