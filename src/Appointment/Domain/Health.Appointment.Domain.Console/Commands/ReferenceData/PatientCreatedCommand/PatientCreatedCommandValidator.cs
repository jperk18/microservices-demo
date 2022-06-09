using FluentValidation;

namespace Health.Appointment.Domain.Console.Commands.ReferenceData.PatientCreatedCommand;

public sealed class PatientCreatedCommandValidator : AbstractValidator<PatientCreatedCommand>
{
    public PatientCreatedCommandValidator()
    {
        RuleFor(patient => patient.Id).NotNull().NotEmpty();
    }
}