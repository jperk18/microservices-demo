using Health.Workflow.Shared.Processes.Core.Models;

namespace Health.Workflow.Shared.Processes;

public class PatientCreated
{
    public PatientCreated()
    {
    }
    public PatientCreated(Guid id, string firstName, string lastName)
    {
        Patient = new Patient() {PatientId = id, FirstName = firstName, LastName = lastName};
    }
    Patient Patient { get; }
}