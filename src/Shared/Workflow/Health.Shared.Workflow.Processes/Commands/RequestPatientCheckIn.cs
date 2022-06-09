using Health.Shared.Workflow.Processes.Core.Exceptions.Models;

namespace Health.Shared.Workflow.Processes.Commands;

public interface RequestPatientCheckIn
{
    Guid AppointmentId { get; }
}

public interface RequestPatientCheckInSuccess
{
    Guid AppointmentId { get; }
}

public interface RequestPatientCheckInFailed
{
    WorkflowValidation Error { get; }
}