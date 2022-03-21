using System.Linq.Expressions;
using Health.Appointment.Domain.StateMachines;
using Health.Appointment.Domain.Storage.Sql.Core.Databases.AppointmentState;
using Health.Appointment.Domain.Storage.Sql.Core.Repository.Core.Generic;

namespace Health.Appointment.Domain.Storage.Sql.Core.Repository.Appointment;

public class AppointmentStateRepository : GenericRepository<AppointmentState>, IAppointmentStateRepository
{
#pragma warning disable CS0108, CS0114
    private readonly AppointmentStateDbContext _context;
#pragma warning restore CS0108, CS0114

    public AppointmentStateRepository(AppointmentStateDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<Guid> GetAllWaitingPatients()
    {
        string waitingState = GetPropertyName((AppointmentStateMachine c) => c.PatientAwaitingNurse);
        return _context.Set<AppointmentState>().Where(x => x.CurrentState == waitingState).Select(c => c.PatientId).ToList();
    }
    
    private static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
    {
        MemberExpression memberExpression = (MemberExpression)property.Body;

        return memberExpression.Member.Name;
    }
}