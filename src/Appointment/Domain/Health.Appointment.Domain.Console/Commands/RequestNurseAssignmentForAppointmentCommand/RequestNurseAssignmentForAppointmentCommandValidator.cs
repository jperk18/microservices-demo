using FluentValidation;

namespace Health.Appointment.Domain.Console.Commands.RequestNurseAssignmentForAppointmentCommand;

public sealed class RequestNurseAssignmentForAppointmentCommandValidator : AbstractValidator<RequestNurseAssignmentForAppointmentCommand>
{
    public RequestNurseAssignmentForAppointmentCommandValidator()
    {
        RuleFor(a => a).NotNull().NotEmpty();
        RuleFor(a => a.Appointment).NotNull().NotEmpty();
        RuleFor(a => a.Nurse).NotNull().NotEmpty();
    }
}