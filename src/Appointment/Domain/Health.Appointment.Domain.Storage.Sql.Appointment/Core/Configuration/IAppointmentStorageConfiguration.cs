using Health.Shared.Domain.Storage.Configuration;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;

public interface AppointmentStorageConfiguration
{
    SqlDatabaseConfiguration AppointmentStateDatabase { get; set; }
}