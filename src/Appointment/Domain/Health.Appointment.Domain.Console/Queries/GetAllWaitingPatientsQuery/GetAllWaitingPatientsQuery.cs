using Health.Shared.Domain.Mediator.Queries;

namespace Health.Appointment.Domain.Console.Queries.GetAllWaitingPatientsQuery;

public sealed class GetAllWaitingPatientsQuery : IQuery<IEnumerable<Guid>>
{
}