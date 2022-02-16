using FluentValidation;

namespace Health.Nurse.Domain.Console.Queries.GetNurseQuery;

public class GetNurseQueryValidator : AbstractValidator<Nurse.Domain.Console.Queries.GetNurseQuery.GetNurseQuery>
{
    public GetNurseQueryValidator()
    {
        RuleFor(patient => patient.PatientId).NotNull().NotEmpty();
    }
}