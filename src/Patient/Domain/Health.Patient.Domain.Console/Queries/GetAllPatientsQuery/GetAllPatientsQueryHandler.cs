using Health.Patient.Domain.Console.Core.Decorators;
using Health.Patient.Domain.Console.Core.Models;
using Health.Patient.Domain.Console.Queries.Core;
using Health.Patient.Domain.Storage.Sql;

namespace Health.Patient.Domain.Console.Queries.GetAllPatientsQuery;

[LoggingPipeline]
[ExceptionPipeline]
public sealed class GetAllPatientsQueryHandler : IQueryHandler<Console.Queries.GetAllPatientsQuery.GetAllPatientsQuery, IEnumerable<PatientRecord>>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public GetAllPatientsQueryHandler(IPatientUnitOfWork unitOfWork)
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