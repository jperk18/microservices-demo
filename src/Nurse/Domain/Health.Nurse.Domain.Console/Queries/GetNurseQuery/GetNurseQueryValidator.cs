using FluentValidation;

namespace Health.Nurse.Domain.Console.Queries.GetNurseQuery;

public class GetNurseQueryValidator : AbstractValidator<Nurse.Domain.Console.Queries.GetNurseQuery.GetNurseQuery>
{
    public GetNurseQueryValidator()
    {
        RuleFor(patient => patient.NurseId).NotNull().NotEmpty();
    }
}

public interface GetNurseQueryI
{
    string Id { get; set; }
}
public class GetNurseQueryIValidator : AbstractValidator<GetNurseQueryI>
{
    public GetNurseQueryIValidator()
    {
        RuleFor(patient => patient.Id).NotNull().NotEmpty();
    }
}