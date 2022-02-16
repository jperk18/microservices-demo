using Health.Patient.Domain.Storage.Sql;

namespace Health.Patient.Domain.Console.Core.Services;

public class PatientRetrievalService : IPatientRetrievalService
{
    private readonly IPatientUnitOfWork _unitOfWork;

    public PatientRetrievalService(IPatientUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> PatientExists(Guid patientId)
    {
        var pat = await _unitOfWork.Patients.GetById(patientId);
        return pat != null;
    }
}