using Health.Patient.Domain.Console.Core.Models;
using Health.Shared.Domain.Queries.Core;

namespace Health.Patient.Domain.Console.Queries.GetAllPatientsQuery;

public sealed class GetAllPatientsQuery : IQuery<IEnumerable<PatientRecord>>
{
}