using Health.Patient.Domain.Storage.Sql.Databases.PatientDb;
using Health.Shared.Domain.Storage.Repository;

namespace Health.Patient.Domain.Storage.Sql;

public class PatientRepository : IPatientRepository
{
    private readonly PatientDbContext _context;

    public PatientRepository(PatientDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Patients = new GenericRepository<Databases.PatientDb.Models.Patient, PatientDbContext>(context);
        //Add additional table repos here
    }

    public IGenericRepository<Databases.PatientDb.Models.Patient> Patients { get; }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}