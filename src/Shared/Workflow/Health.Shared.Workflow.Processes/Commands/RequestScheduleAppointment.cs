using Health.Shared.Workflow.Processes.Exceptions.Models;

namespace Health.Shared.Workflow.Processes.Commands;

public interface RequestScheduleAppointment
{
    Guid PatientId { get; }
}

public interface RequestScheduleAppointmentSuccess
{
    Guid AppointmentId { get; }
}

public interface RequestScheduleAppointmentFailed
{
    WorkflowValidation Error { get; }
}