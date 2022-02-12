using Health.Patient.Domain.Storage.Sql.Core.Repository.PatientDb;

namespace Health.Patient.Domain.Storage.Sql;

public interface IPatientUnitOfWork : IDisposable
{
    IPatientRepository Patients { get; }
    Task<int> Complete();
}