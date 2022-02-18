using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Events;

public interface NurseCreated
{
    Nurse Nurse { get; }
}