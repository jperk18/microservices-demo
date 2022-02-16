using Health.Patient.Domain.Console.Core.Models;
using Health.Patient.Domain.Storage.Sql;
using Health.Shared.Domain.Core.Decorators;
using Health.Shared.Domain.Queries.Core;

namespace Health.Patient.Domain.Console.Queries.GetAllPatientsQuery;

[LoggingPipeline]
[ExceptionPipeline]
public sealed class GetAllPatientsAsyncQueryHandler : IAsyncQueryHandler<Console.Queries.GetAllPatientsQuery.GetAllPatientsQuery, IEnumerable<PatientRecord>>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public GetAllPatientsAsyncQueryHandler(IPatientUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<IEnumerable<PatientRecord>> Handle(Console.Queries.GetAllPatientsQuery.GetAllPatientsQuery command)
    {
        return await Task.FromResult(
            _unitOfWork.Patients.GetAll().Select(i => new PatientRecord(i.FirstName, i.LastName, i.DateOfBirth, i.Id))
            );
    }
}