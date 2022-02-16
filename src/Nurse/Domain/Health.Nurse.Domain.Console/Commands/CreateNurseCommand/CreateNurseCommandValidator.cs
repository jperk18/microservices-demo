using FluentValidation;

namespace Health.Nurse.Domain.Console.Commands.CreateNurseCommand;

public sealed class CreateNurseCommandValidator : AbstractValidator<CreateNurseCommand>
{
    public CreateNurseCommandValidator()
    {
        RuleFor(patient => patient.FirstName).NotNull().NotEmpty();
        RuleFor(patient => patient.LastName).NotNull().NotEmpty();
    }
}