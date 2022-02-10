namespace Health.Workflow.Processes.Core.Models;

public class Patient
{
    public Guid PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
}