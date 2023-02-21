using Health.Nurse.Domain.Console.Core.Exceptions;
using Health.Nurse.Domain.Console.Core.Models;
using Health.Nurse.Domain.Storage.Sql;
using Health.Shared.Domain.Exceptions.Models;
using Health.Shared.Domain.Mediator.Decorators;
using Health.Shared.Domain.Mediator.Queries;

namespace Health.Nurse.Domain.Console.Queries.GetNurseQuery;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
public sealed class GetNurseAsyncQueryHandler : IAsyncQueryHandler<GetNurseQuery, NurseRecord>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetNurseAsyncQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<NurseRecord> Handle(GetNurseQuery command)
    {
        var i = await _unitOfWork.Nurses.GetById(command.NurseId);

        if (i == null)
            throw new NurseDomainValidationException($"Record does not exist for {command.NurseId}", new DomainValidationFailureDto[]{new("0001", "Nurse does not exist")});
        
        return new NurseRecord(i.FirstName, i.LastName, i.DateOfBirth, i.Id);
    }
}