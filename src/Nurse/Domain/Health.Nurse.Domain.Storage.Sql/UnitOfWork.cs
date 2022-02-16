using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;
using Health.Nurse.Domain.Storage.Sql.Core.Repository.NurseDb;

namespace Health.Nurse.Domain.Storage.Sql;

public class UnitOfWork : IUnitOfWork
{
    private readonly NurseDbContext _context;

    public UnitOfWork(NurseDbContext context)
    {
        _context = context;
        Nurses = new NurseRepository(_context);
        //Add additional table repos here
    }

    public INurseRepository Nurses { get; private set; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}