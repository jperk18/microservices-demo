using System.Linq.Expressions;
using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Shared.Domain.Storage.Repository;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Repository.Generic;

public class GenericAppointmentStateDbQueryRepository<T> : IGenericQueryRepository<T> where T : class
{
    protected readonly AppointmentStateDbContext _context;
    public GenericAppointmentStateDbQueryRepository(AppointmentStateDbContext context)
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