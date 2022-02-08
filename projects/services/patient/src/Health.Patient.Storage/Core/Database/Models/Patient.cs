using System.ComponentModel.DataAnnotations;

namespace Health.Patient.Storage.Core.Database.Models;

public class Patient
{
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}