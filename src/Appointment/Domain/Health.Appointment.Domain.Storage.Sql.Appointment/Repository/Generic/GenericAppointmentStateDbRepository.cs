using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Shared.Domain.Storage.Repository;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Repository.Generic;

public class GenericAppointmentStateDbRepository<T> : GenericAppointmentStateDbQueryRepository<T>, IGenericRepository<T> where T : class
{
#pragma warning disable CS0108, CS0114
    private readonly AppointmentStateDbContext _context;
#pragma warning restore CS0108, CS0114
    public GenericAppointmentStateDbRepository(AppointmentStateDbContext context) : base(context)
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

    public async Task<T> Update(T entity)
    {
        await Task.Run(() =>
        {
            _context.Set<T>().Update(entity);
        });
        
        return entity;
    }

    public async Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities)
    {
        await Task.Run(() =>
        {
            _context.Set<T>().UpdateRange(entities);
        });
        
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

    public async Task<T> AddOrUpdate(Guid id, T entity)
    {
        var obj = await GetById(id);
        if (obj == null) return await Add(entity);
        return await Update(entity);
    }
}