using Health.Shared.Workflow.Processes.Core.Exceptions.Models;
using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Commands;

public interface RegisterNurse : NurseInfo
{
}

public interface RegisterNurseSuccess : Nurse
{
}

public interface RegisterNurseFailed
{
    WorkflowValidation Error { get; }
}