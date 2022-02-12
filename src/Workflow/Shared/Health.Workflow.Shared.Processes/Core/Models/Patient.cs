namespace Health.Workflow.Shared.Processes.Core.Models;

public class Patient
{
    public Guid PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}