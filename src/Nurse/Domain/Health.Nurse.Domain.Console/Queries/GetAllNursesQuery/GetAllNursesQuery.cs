using Health.Nurse.Domain.Console.Core.Models;
using Health.Shared.Domain.Queries.Core;

namespace Health.Nurse.Domain.Console.Queries.GetAllNursesQuery;

public sealed class GetAllNursesQuery : IQuery<IEnumerable<NurseRecord>>
{
}