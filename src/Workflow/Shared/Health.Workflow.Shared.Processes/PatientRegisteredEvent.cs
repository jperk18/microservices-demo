using Health.Workflow.Shared.Processes.Core.Models;

namespace Health.Workflow.Shared.Processes;

public class PatientRegisteredEvent
{
    public PatientRegisteredEvent()
    {
    }
    
    public Patient Patient { get; set; }
}