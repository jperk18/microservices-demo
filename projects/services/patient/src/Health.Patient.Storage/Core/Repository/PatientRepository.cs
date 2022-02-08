using Health.Patient.Storage.Core.Database;
using Health.Patient.Storage.Core.Repository.Generic;

namespace Health.Patient.Storage;

public class PatientRepository : GenericRepository<Core.Database.Models.Patient>, IPatientRepository
{
    private readonly PatientDbContext _context;

    public PatientRepository(PatientDbContext context) : base(context)
    {
        _context = context;
    }
}