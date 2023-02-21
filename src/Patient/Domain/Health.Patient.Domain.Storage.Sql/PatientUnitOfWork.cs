using Health.Patient.Domain.Storage.Sql.Core.Databases.PatientDb;
using Health.Shared.Domain.Storage.Repository;

namespace Health.Patient.Domain.Storage.Sql;

public class PatientUnitOfWork : IPatientUnitOfWork
{
    private readonly PatientDbContext _context;

    public PatientUnitOfWork(PatientDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Patients = new GenericRepository<Domain.Storage.Sql.Core.Databases.PatientDb.Models.Patient, PatientDbContext>(context);
        //Add additional table repos here
    }

    public IGenericRepository<Core.Databases.PatientDb.Models.Patient> Patients { get; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}