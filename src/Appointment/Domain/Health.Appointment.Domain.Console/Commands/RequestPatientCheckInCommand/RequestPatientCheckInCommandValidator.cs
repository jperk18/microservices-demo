using FluentValidation;

namespace Health.Appointment.Domain.Console.Commands.RequestPatientCheckInCommand;

public sealed class RequestPatientCheckInCommandValidator : AbstractValidator<RequestPatientCheckInCommand>
{
    public RequestPatientCheckInCommandValidator()
    {
        RuleFor(a => a).NotNull().NotEmpty();
        RuleFor(a => a.Appointment).NotNull().NotEmpty();
    }
}