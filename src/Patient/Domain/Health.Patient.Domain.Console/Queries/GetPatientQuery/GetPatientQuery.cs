using Health.Patient.Domain.Console.Core.Models;
using Health.Shared.Domain.Queries.Core;

namespace Health.Patient.Domain.Console.Queries.GetPatientQuery;

public sealed class GetPatientQuery : IQuery<PatientRecord>
{
    public GetPatientQuery(Guid patientId)
    {
        PatientId = patientId;
    }
    public Guid PatientId { get; set; }
}