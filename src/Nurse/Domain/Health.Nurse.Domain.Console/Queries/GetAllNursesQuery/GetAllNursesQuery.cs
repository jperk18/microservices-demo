using Health.Nurse.Domain.Console.Core.Models;
using Health.Shared.Domain.Mediator.Queries;

namespace Health.Nurse.Domain.Console.Queries.GetAllNursesQuery;

public sealed class GetAllNursesQuery : IQuery<IEnumerable<NurseRecord>>
{
}