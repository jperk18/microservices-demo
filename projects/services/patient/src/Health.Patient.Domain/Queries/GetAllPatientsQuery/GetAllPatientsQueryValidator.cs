using FluentValidation;

namespace Health.Patient.Domain.Queries.GetPatientQuery;

public class GetAllPatientsQueryValidator : AbstractValidator<GetPatientQuery>
{
    public GetAllPatientsQueryValidator()
    {
    }
}