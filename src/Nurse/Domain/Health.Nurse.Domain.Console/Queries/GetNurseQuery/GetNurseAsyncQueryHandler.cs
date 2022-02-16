using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Nurse.Domain.Console.Core.Exceptions;
using Health.Nurse.Domain.Console.Core.Models;
using Health.Nurse.Domain.Storage.Sql;
using Health.Shared.Domain.Core.Decorators;
using Health.Shared.Domain.Queries.Core;

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
        //TODO: More Business logic
        
        var i = await _unitOfWork.Nurses.GetById(command.PatientId);

        if (i == null)
            throw new NurseDomainValidationException($"Record does not exist for {command.PatientId}");
        
        return new NurseRecord(i.FirstName, i.LastName, i.DateOfBirth, i.Id);
    }
}