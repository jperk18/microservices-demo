using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Database;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Patient;

namespace Health.Appointment.Domain.Storage.UnitOfWorks;

public class RefDataUnitOfWork : IRefDataUnitOfWork
{
    private readonly ReferenceDataDbContext _context;

    public RefDataUnitOfWork(ReferenceDataDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        PatientReferenceData = new PatientReferenceDataRepository(_context);
    }

    public IPatientReferenceDataRepository PatientReferenceData { get; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}