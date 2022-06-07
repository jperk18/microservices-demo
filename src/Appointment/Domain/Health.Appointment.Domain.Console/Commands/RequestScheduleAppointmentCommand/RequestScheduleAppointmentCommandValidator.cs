using FluentValidation;

namespace Health.Appointment.Domain.Console.Commands.RequestScheduleAppointmentCommand;

public sealed class RequestScheduleAppointmentCommandValidator : AbstractValidator<RequestScheduleAppointmentCommand>
{
    public RequestScheduleAppointmentCommandValidator()
    {
        RuleFor(a => a).NotNull().NotEmpty();
        RuleFor(a => a.Patient).NotNull().NotEmpty();
    }
}