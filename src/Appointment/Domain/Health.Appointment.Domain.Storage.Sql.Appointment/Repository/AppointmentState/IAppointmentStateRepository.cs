using Health.Shared.Domain.Storage.Repository;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Repository.AppointmentState;

public interface IAppointmentStateRepository : IGenericRepository<StateMachines.AppointmentState>
{
    Task<IEnumerable<Guid>?> GetWaitingPatients();
    Task<IEnumerable<Guid>?> GetScheduledAppointments();
}