using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Patient.Domain.Console.Core.Models;
using Health.Patient.Domain.Storage.Sql;
using Health.Shared.Domain.Core.Decorators;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Queries.Core;

namespace Health.Patient.Domain.Console.Queries.GetPatientQuery;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
public sealed class GetPatientAsyncQueryHandler : IAsyncQueryHandler<Console.Queries.GetPatientQuery.GetPatientQuery, PatientRecord>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public GetPatientAsyncQueryHandler(IPatientUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<PatientRecord> Handle(Console.Queries.GetPatientQuery.GetPatientQuery command)
    {
        var i = await _unitOfWork.Patients.GetById(command.PatientId);

        if (i == null)
            throw new DomainValidationException($"Patient does not exist for {command.PatientId}");
        
        return new PatientRecord(i.FirstName, i.LastName, i.DateOfBirth, i.Id);
    }
}