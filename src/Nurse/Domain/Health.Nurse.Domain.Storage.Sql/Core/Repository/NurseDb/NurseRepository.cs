using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;
using Health.Nurse.Domain.Storage.Sql.Core.Repository.Core.Generic;

namespace Health.Nurse.Domain.Storage.Sql.Core.Repository.NurseDb;

public class NurseRepository : GenericRepository<Nurse.Domain.Storage.Sql.Core.Databases.NurseDb.Models.Nurse>, INurseRepository
{
#pragma warning disable CS0108, CS0114
    private readonly NurseDbContext _context;
#pragma warning restore CS0108, CS0114

    public NurseRepository(NurseDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}