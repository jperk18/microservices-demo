using FluentValidation;
using Health.Shared.Workflow.Processes.Commands;

namespace Health.Appointment.Domain.Console.Validators;

public class RequestPatientCheckInValidator : AbstractValidator<RequestPatientCheckIn>
{
    public RequestPatientCheckInValidator()
    {
        RuleFor(a => a).NotNull().NotEmpty();
        RuleFor(a => a.AppointmentId).NotNull().NotEmpty();
    }
}