using FluentValidation;

namespace Health.Patient.Domain.Console.Commands.CreatePatientCommand;

public sealed class CreatePatientCommandValidator : AbstractValidator<Console.Commands.CreatePatientCommand.CreatePatientCommand>
{
    public CreatePatientCommandValidator()
    {
        RuleFor(patient => patient.FirstName).NotNull().NotEmpty();
        RuleFor(patient => patient.LastName).NotNull().NotEmpty();
    }
}