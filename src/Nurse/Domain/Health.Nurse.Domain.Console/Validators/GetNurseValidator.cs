using FluentValidation;
using Health.Shared.Workflow.Processes.Queries;

namespace Health.Nurse.Domain.Console.Validators;

public class GetNurseValidator : AbstractValidator<GetNurse>
{
    public GetNurseValidator()
    {
        RuleFor(nurse => nurse).NotNull().NotEmpty();
        RuleFor(nurse => nurse.Id).NotNull().NotEmpty();
    }
}