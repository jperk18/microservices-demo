using FluentValidation;
using Health.Shared.Workflow.Processes.Commands;

namespace Health.Appointment.Domain.Console.Validators;

public sealed class RequestNurseAssignmentForAppointmentValidator : AbstractValidator<RequestNurseAssignmentForAppointment>
{
    public RequestNurseAssignmentForAppointmentValidator()
    {
        RuleFor(a => a).NotNull().NotEmpty();
        RuleFor(a => a.AppointmentId).NotNull().NotEmpty();
        RuleFor(a => a.NurseId).NotNull().NotEmpty();
    }
}