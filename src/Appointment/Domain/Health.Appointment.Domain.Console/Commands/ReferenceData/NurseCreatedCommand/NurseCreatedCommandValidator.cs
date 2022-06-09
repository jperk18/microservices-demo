using FluentValidation;

namespace Health.Appointment.Domain.Console.Commands.ReferenceData.NurseCreatedCommand;

public sealed class NurseCreatedCommandValidator : AbstractValidator<NurseCreatedCommand>
{
    public NurseCreatedCommandValidator()
    {
        RuleFor(n => n.Id).NotNull().NotEmpty();
    }
}