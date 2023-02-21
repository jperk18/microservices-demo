using Health.Shared.Domain.Storage.Repository;

namespace Health.Appointment.Domain.Storage.Sql.Appointment;

public interface IAppointmentRepository : IDisposable
{
    IGenericRepository<StateMachines.AppointmentState> AppointmentState { get; }
    Task<IEnumerable<Guid>?> GetWaitingPatients();
    Task<IEnumerable<Guid>?> GetScheduledAppointments();
    Task<int> Complete();
}