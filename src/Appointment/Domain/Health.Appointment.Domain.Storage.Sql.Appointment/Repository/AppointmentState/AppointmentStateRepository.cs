using System.Linq.Expressions;
using Health.Appointment.Domain.StateMachines;
using Health.Appointment.Domain.Storage.Sql.Appointment.Database;
using Health.Appointment.Domain.Storage.Sql.Appointment.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Repository.AppointmentState;

public class AppointmentStateRepository : GenericAppointmentStateDbRepository<StateMachines.AppointmentState>, IAppointmentStateRepository
{
#pragma warning disable CS0108, CS0114
    private readonly AppointmentStateDbContext _context;
#pragma warning restore CS0108, CS0114

    public AppointmentStateRepository(AppointmentStateDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Guid>?> GetWaitingPatients()
    {
        string waitingState = GetPropertyName((AppointmentStateMachine c) => c.PatientAwaitingNurse);
        return await GetPatients(waitingState);
    }
    
    public async Task<IEnumerable<Guid>?> GetScheduledAppointments()
    {
        string appointmentScheduled = GetPropertyName((AppointmentStateMachine c) => c.AppointmentScheduled);
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
    
    private static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
    {
        MemberExpression memberExpression = (MemberExpression)property.Body;

        return memberExpression.Member.Name;
    }
}