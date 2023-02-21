using Health.Patient.Domain.Console.Core.Models;
using Health.Shared.Domain.Mediator.Queries;

namespace Health.Patient.Domain.Console.Queries.GetAllPatientsQuery;

public sealed class GetAllPatientsQuery : IQuery<IEnumerable<PatientRecord>>
{
}