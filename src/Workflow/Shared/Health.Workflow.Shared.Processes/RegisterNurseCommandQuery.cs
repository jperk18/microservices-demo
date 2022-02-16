namespace Health.Workflow.Shared.Processes;

public class RegisterNurseCommandQuery
{
    public RegisterNurseCommandQuery()
    {
    }

    public RegisterNurseCommandQuery(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}