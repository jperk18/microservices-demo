using Health.Patient.Storage.Core.Database;

namespace Health.Patient.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly PatientDbContext _context;

    public UnitOfWork(PatientDbContext context)
    {
        _context = context;
        Patients = new PatientRepository(_context);
    }

    public IPatientRepository Patients { get; private set; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}