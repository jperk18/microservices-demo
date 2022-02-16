using Health.Workflow.Shared.Processes.Core.Exceptions.Models;

namespace Health.Workflow.Shared.Processes;

public interface CheckInPatientSuccessResponse
{
    public Guid PatientId { get; set; }
}

public interface CheckInPatientFailResponse
{
    Guid PatientId { get; }

    WorkflowValidation Error { get; }
}