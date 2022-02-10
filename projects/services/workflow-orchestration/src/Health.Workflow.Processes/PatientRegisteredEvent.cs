using Health.Workflow.Processes.Core.Models;

namespace Health.Workflow.Processes;

public class PatientRegisteredEvent
{
    public Patient Patient { get; set; }
}