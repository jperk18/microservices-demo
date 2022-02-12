using Health.Patient.Domain.Storage.Sql.Core.Databases.PatientDb;
using Health.Patient.Domain.Storage.Sql.Core.Repository.PatientDb;

namespace Health.Patient.Domain.Storage.Sql;

public class PatientUnitOfWork : IPatientUnitOfWork
{
    private readonly PatientDbContext _context;

    public PatientUnitOfWork(PatientDbContext context)
    {
        _context = context;
        Patients = new PatientRepository(_context);
        //Add additional table repos here
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