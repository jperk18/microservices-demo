using Health.Nurse.Domain.Console.Core.Models;
using Health.Nurse.Domain.Storage.Sql;
using Health.Shared.Domain.Core.Decorators;
using Health.Shared.Domain.Queries.Core;

namespace Health.Nurse.Domain.Console.Queries.GetAllNursesQuery;

[LoggingPipeline]
[ExceptionPipeline]
public sealed class GetAllNursesAsyncQueryHandler : IAsyncQueryHandler<GetAllNursesQuery, IEnumerable<NurseRecord>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllNursesAsyncQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<IEnumerable<NurseRecord>> Handle(GetAllNursesQuery command)
    {
        var r = _unitOfWork.Nurses.GetAll();
        var nurses = r as Storage.Sql.Core.Databases.NurseDb.Models.Nurse[] ?? r.ToArray();

        return await Task.FromResult(
            nurses.Select(i => new NurseRecord(i.FirstName, i.LastName, i.DateOfBirth, i.Id))
            );
    }
}