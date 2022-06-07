using Health.Shared.Workflow.Processes.Core.Exceptions.Models;

namespace Health.Shared.Workflow.Processes.Commands;

public interface RequestNurseAssignmentForAppointment
{
    Guid AppointmentId { get; }
    Guid NurseId { get; }
}

public interface RequestNurseAssignmentForAppointmentSuccess : RequestNurseAssignmentForAppointment
{
    bool Assigned { get; set; }
}

public interface RequestNurseAssignmentForAppointmentFailed
{
    WorkflowValidation Error { get; }
}