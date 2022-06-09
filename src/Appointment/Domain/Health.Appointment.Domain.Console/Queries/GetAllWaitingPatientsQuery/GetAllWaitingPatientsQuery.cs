using Health.Shared.Domain.Queries.Core;

namespace Health.Appointment.Domain.Console.Queries.GetAllWaitingPatientsQuery;

public sealed class GetAllWaitingPatientsQuery : IQuery<IEnumerable<Guid>>
{
}