using Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Patient;

namespace Health.Appointment.Domain.Storage.UnitOfWorks;

public interface IRefDataUnitOfWork : IDisposable
{
    IPatientReferenceDataRepository PatientReferenceData { get; }
    Task<int> Complete();
}