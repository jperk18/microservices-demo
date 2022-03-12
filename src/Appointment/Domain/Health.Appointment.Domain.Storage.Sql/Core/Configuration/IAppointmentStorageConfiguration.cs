using Health.Appointment.Domain.Storage.Sql.Core.Configuration.Inner;

namespace Health.Appointment.Domain.Storage.Sql.Core.Configuration;

public interface IAppointmentStorageConfiguration
{
    SqlDatabaseConfiguration AppointmentStateDatabase { get; set; }
}