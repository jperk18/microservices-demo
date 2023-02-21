using Microsoft.EntityFrameworkCore;

namespace Health.Shared.Domain.Storage.Repository;

public class GenericRepository<T, TDbContext> : GenericQueryRepository<T, TDbContext>, IGenericRepository<T> 
    where T : class
    where TDbContext : DbContext
{
#pragma warning disable CS0108, CS0114
    private readonly TDbContext _context;
#pragma warning restore CS0108, CS0114
    public GenericRepository(TDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
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

    public Task<T> Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return Task.FromResult(entity);
    }

    public Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities)
    {
        _context.Set<T>().UpdateRange(entities);
        return Task.FromResult(entities);
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public async Task<T> AddOrUpdate(T entity)
    {
        var item = await FindAsync(entity);
        if (item == null)
        {
            await Add(entity);
        }
        else
        {
            await Update(entity);
        }

        return entity;
    }
}