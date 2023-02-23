using FluentValidation;
using Health.Shared.Workflow.Processes.Commands;

namespace Health.Appointment.Domain.Console.Validators;

public sealed class RequestScheduleAppointmentValidator : AbstractValidator<RequestScheduleAppointment>
{
    public RequestScheduleAppointmentValidator()
    {
        RuleFor(a => a).NotNull().NotEmpty();
        RuleFor(a => a.PatientId).NotNull().NotEmpty();
    }
}