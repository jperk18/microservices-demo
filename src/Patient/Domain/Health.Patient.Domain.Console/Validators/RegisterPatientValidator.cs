using FluentValidation;
using Health.Shared.Workflow.Processes.Commands;

namespace Health.Patient.Domain.Console.Validators;

public sealed class RegisterPatientValidator : AbstractValidator<RegisterPatient>
{
    public RegisterPatientValidator()
    {
        RuleFor(patient => patient).NotNull().NotEmpty();
        RuleFor(patient => patient.FirstName).NotNull().NotEmpty();
        RuleFor(patient => patient.LastName).NotNull().NotEmpty();
    }
}