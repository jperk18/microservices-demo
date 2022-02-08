using Health.Patient.Storage.Core.Database;

namespace Health.Patient.Storage.Core.Repository.Generic;

public class GenericRepository<T> : GenericQueryRepository<T>, IGenericRepository<T> where T : class
{
    protected readonly PatientDbContext _context;
    public GenericRepository(PatientDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<T> Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }
    public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        return entities;
    }
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}