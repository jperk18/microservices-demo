using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Health.Shared.Domain.Storage.Repository;

public class GenericQueryRepository<T, TDbContext> : IGenericQueryRepository<T> 
    where T : class
    where TDbContext : DbContext
{
    protected readonly TDbContext _context;
    public GenericQueryRepository(TDbContext context)
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
    
    public async Task<T?> FindAsync(T entity)
    {
        return await _context.FindAsync<T>(entity);
    }
}