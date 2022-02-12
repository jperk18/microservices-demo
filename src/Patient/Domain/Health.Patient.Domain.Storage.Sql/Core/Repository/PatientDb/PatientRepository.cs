using Health.Patient.Domain.Storage.Sql.Core.Databases.PatientDb;
using Health.Patient.Domain.Storage.Sql.Core.Repository.Core.Generic;

namespace Health.Patient.Domain.Storage.Sql.Core.Repository.PatientDb;

public class PatientRepository : GenericRepository<Domain.Storage.Sql.Core.Databases.PatientDb.Models.Patient>, IPatientRepository
{
#pragma warning disable CS0108, CS0114
    private readonly PatientDbContext _context;
#pragma warning restore CS0108, CS0114

    public PatientRepository(PatientDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}