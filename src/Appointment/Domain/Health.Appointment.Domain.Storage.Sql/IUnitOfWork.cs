using Health.Appointment.Domain.Storage.Sql.Core.Repository.Appointment;

namespace Health.Appointment.Domain.Storage.Sql;

public interface IUnitOfWork : IDisposable
{
    IAppointmentStateRepository AppointmentState { get; }
    Task<int> Complete();
}