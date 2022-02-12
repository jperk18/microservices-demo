using Health.Patient.Domain.Console.Core.Decorators;
using Health.Patient.Domain.Console.Core.Exceptions;
using Health.Patient.Domain.Console.Core.Models;
using Health.Patient.Domain.Console.Queries.Core;
using Health.Patient.Domain.Storage.Sql;

namespace Health.Patient.Domain.Console.Queries.GetPatientQuery;

[AuditLogPipeline]
[ExceptionPipeline]
[ValidationPipeline]
public sealed class GetPatientQueryHandler : IQueryHandler<Console.Queries.GetPatientQuery.GetPatientQuery, PatientRecord>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public GetPatientQueryHandler(IPatientUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<PatientRecord> Handle(Console.Queries.GetPatientQuery.GetPatientQuery command)
    {
        //TODO: More Business logic
        
        var i = await _unitOfWork.Patients.GetById(command.PatientId);

        if (i == null)
            throw new DomainValidationException($"Record does not exist for {command.PatientId}");
        
        return new PatientRecord(i.FirstName, i.LastName, i.DateOfBirth, i.Id);
    }
}