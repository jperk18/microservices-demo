namespace Health.Workflow.Shared.Processes;

public class CheckInPatientCommandQuery
{
    public CheckInPatientCommandQuery()
    {
    }
    
    public CheckInPatientCommandQuery(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; set; }
}