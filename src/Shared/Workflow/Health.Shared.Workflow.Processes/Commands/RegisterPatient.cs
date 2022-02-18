using Health.Shared.Workflow.Processes.Core.Exceptions.Models;
using Health.Shared.Workflow.Processes.Inner.Models;

namespace Health.Shared.Workflow.Processes.Commands;

public interface RegisterPatient : PatientInfo
{
}

public interface RegisterPatientSuccess : Patient
{
}

public interface RegisterPatientFailed
{
    WorkflowValidation Error { get; }
}