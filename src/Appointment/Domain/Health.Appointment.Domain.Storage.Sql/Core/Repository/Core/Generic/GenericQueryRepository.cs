using System.Linq.Expressions;
using Health.Appointment.Domain.Storage.Sql.Core.Databases.AppointmentState;

namespace Health.Appointment.Domain.Storage.Sql.Core.Repository.Core.Generic;

public class GenericQueryRepository<T> : IGenericQueryRepository<T> where T : class
{
    protected readonly AppointmentStateDbContext _context;
    public GenericQueryRepository(AppointmentStateDbContext context)
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