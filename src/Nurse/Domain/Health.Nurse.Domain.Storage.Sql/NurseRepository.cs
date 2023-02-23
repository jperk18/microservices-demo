using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;
using Health.Nurse.Domain.Storage.Sql.Databases.NurseDb;
using Health.Shared.Domain.Storage.Repository;

namespace Health.Nurse.Domain.Storage.Sql;

public class NurseRepository : INurseRepository
{
    private readonly NurseDbContext _context;

    public NurseRepository(NurseDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Nurses = new GenericRepository<Core.Databases.NurseDb.Models.Nurse, NurseDbContext>(context);
    }

    public IGenericRepository<Core.Databases.NurseDb.Models.Nurse> Nurses { get; private set; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}