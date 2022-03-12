using Health.Appointment.Domain.Storage.Sql.Core.Databases.AppointmentState;
using Health.Appointment.Domain.Storage.Sql.Core.Repository.Appointment;

namespace Health.Appointment.Domain.Storage.Sql;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppointmentStateDbContext _context;

    public UnitOfWork(AppointmentStateDbContext context)
    {
        _context = context;
        AppointmentState = new AppointmentStateRepository(_context);
        //Add additional table repos here
    }

    public IAppointmentStateRepository AppointmentState { get; private set; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}