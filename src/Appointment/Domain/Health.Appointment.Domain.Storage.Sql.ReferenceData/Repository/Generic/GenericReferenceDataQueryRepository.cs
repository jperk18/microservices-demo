using System.Linq.Expressions;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Database;
using Health.Shared.Domain.Storage.Repository;

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Generic;

public class GenericReferenceDataQueryRepository<T> : IGenericQueryRepository<T> where T : class
{
    protected readonly ReferenceDataDbContext _context;
    public GenericReferenceDataQueryRepository(ReferenceDataDbContext context)
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