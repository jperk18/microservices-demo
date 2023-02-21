using Health.Appointment.Domain.StateMachines;
using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Shared.Application.Core;
using Health.Shared.Domain.Storage.Repository;
using Microsoft.EntityFrameworkCore;

namespace Health.Appointment.Domain.Storage.Sql.Appointment;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentStateDbContext _context;

    public AppointmentRepository(AppointmentStateDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        AppointmentState = new GenericRepository<StateMachines.AppointmentState, AppointmentStateDbContext>(context);
    }

    public IGenericRepository<StateMachines.AppointmentState> AppointmentState { get; }

    public async Task<IEnumerable<Guid>?> GetWaitingPatients()
    {
        string waitingState = Reflection.GetPropertyName((AppointmentStateMachine c) => c.PatientAwaitingNurse);
        return await GetPatients(waitingState);
    }
    
    public async Task<IEnumerable<Guid>?> GetScheduledAppointments()
    {
        string appointmentScheduled = Reflection.GetPropertyName((AppointmentStateMachine c) => c.AppointmentScheduled);
        return await GetAppointments(appointmentScheduled);
    }
    
    private async Task<IEnumerable<Guid>?> GetPatients(string state)
    {
        return await _context.Set<StateMachines.AppointmentState>().Where(x => x.CurrentState == state).Select(c => c.PatientId).ToListAsync();
    }
    
    private async Task<IEnumerable<Guid>?> GetAppointments(string state)
    {
        return await _context.Set<StateMachines.AppointmentState>().Where(x => x.CurrentState == state).Select(c => c.CorrelationId).ToListAsync();
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}