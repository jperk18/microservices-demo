using Health.Shared.Domain.Storage.Repository;

namespace Health.Patient.Domain.Storage.Sql;

public interface IPatientUnitOfWork : IDisposable
{
    IGenericRepository<Domain.Storage.Sql.Core.Databases.PatientDb.Models.Patient> Patients { get; }
    Task<int> Complete();
}