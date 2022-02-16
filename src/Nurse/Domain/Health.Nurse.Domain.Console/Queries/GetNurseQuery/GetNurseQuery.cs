using Health.Nurse.Domain.Console.Core.Models;
using Health.Nurse.Domain.Console.Queries.Core;

namespace Health.Nurse.Domain.Console.Queries.GetNurseQuery;

public sealed class GetNurseQuery : IQuery<NurseRecord>
{
    public GetNurseQuery(Guid patientId)
    {
        PatientId = patientId;
    }
    public Guid PatientId { get; set; }
}