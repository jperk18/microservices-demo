using Health.Patient.Domain.Console.Core.Models;
using Health.Shared.Domain.Mediator.Queries;

namespace Health.Patient.Domain.Console.Queries.GetPatientQuery;

public sealed class GetPatientQuery : IQuery<PatientRecord>
{
    public GetPatientQuery(Guid patientId)
    {
        PatientId = patientId;
    }
    public Guid PatientId { get; set; }
}