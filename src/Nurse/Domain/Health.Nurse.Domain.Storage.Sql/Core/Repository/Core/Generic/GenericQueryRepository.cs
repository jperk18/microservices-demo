using System.Linq.Expressions;
using Health.Nurse.Domain.Storage.Sql.Core.Databases.NurseDb;

namespace Health.Nurse.Domain.Storage.Sql.Core.Repository.Core.Generic;

public class GenericQueryRepository<T> : IGenericQueryRepository<T> where T : class
{
    protected readonly NurseDbContext _context;
    public GenericQueryRepository(NurseDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }
    public async Task<T?> GetById(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
}