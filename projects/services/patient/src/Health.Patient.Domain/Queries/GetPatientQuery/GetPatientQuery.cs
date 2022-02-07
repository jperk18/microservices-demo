using Health.Patient.Domain.Queries.Core;

namespace Health.Patient.Domain.Queries.GetPatientQuery;

public sealed class GetPatientQuery : IQuery<string>
{
    public Guid PatientId { get; set; }
}