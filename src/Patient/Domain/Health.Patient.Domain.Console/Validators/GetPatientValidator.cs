using FluentValidation;
using Health.Shared.Workflow.Processes.Queries;

namespace Health.Patient.Domain.Console.Validators;

public class GetPatientValidator : AbstractValidator<GetPatient>
{
    public GetPatientValidator()
    {
        RuleFor(patient => patient).NotNull().NotEmpty();
        RuleFor(patient => patient.Id).NotNull().NotEmpty();
    }
}