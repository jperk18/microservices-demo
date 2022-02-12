using FluentValidation;

namespace Health.Patient.Domain.Console.Queries.GetPatientQuery;

public class GetPatientQueryValidator : AbstractValidator<Console.Queries.GetPatientQuery.GetPatientQuery>
{
    public GetPatientQueryValidator()
    {
        RuleFor(patient => patient.PatientId).NotNull().NotEmpty();
    }
}