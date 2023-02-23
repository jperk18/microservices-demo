using FluentValidation;
using Health.Shared.Workflow.Processes.Commands;

namespace Health.Nurse.Domain.Console.Validators;

public sealed class RegisterNurseValidator : AbstractValidator<RegisterNurse>
{
    public RegisterNurseValidator()
    {
        RuleFor(patient => patient.FirstName).NotNull().NotEmpty();
        RuleFor(patient => patient.LastName).NotNull().NotEmpty();
    }
}