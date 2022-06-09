using Health.Shared.Domain.Storage.Configuration;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;

public class AppointmentStorageConfiguration : IAppointmentStorageConfiguration
{
    public AppointmentStorageConfiguration(SqlDatabaseConfiguration appDatabaseConfiguration)
    {
        AppointmentStateDatabase = appDatabaseConfiguration ?? throw new ArgumentNullException(nameof(appDatabaseConfiguration));
    }
    public SqlDatabaseConfiguration AppointmentStateDatabase { get; set; }
}