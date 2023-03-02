using Health.Shared.Domain.Storage.Repository;

namespace Health.Patient.Domain.Storage.Sql;

public interface IPatientRepository : IDisposable
{
    IGenericRepository<Databases.PatientDb.Models.Patient> Patients { get; }

    Task<int> SaveChangesAsync();
}