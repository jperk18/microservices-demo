namespace Health.Patient.Storage;

public interface IUnitOfWork : IDisposable
{
    IPatientRepository Patients { get; }
    Task<int> Complete();
}