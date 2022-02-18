using Health.Workflow.Shared.Processes.Core.Models;

namespace Health.Workflow.Shared.Processes;

public class NurseCreated
{
    public NurseCreated()
    {
    }
    public NurseCreated(Guid id, string firstName, string lastName)
    {
        Nurse = new Nurse() {Id = id, FirstName = firstName, LastName = lastName};
    }
    Nurse Nurse { get; }
}