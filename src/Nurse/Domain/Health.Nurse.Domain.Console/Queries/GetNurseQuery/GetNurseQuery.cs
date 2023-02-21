using Health.Nurse.Domain.Console.Core.Models;
using Health.Shared.Domain.Mediator.Queries;

namespace Health.Nurse.Domain.Console.Queries.GetNurseQuery;

public sealed class GetNurseQuery : IQuery<NurseRecord>
{
    public GetNurseQuery(Guid nurseId)
    {
        NurseId = nurseId;
    }
    public Guid NurseId { get; set; }
}