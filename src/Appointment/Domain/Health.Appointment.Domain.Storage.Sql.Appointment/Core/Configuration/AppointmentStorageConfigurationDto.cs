using Health.Shared.Domain.Storage.Configuration;

namespace Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;

public class AppointmentStorageConfigurationDto : AppointmentStorageConfiguration
{
    public AppointmentStorageConfigurationDto(SqlDatabaseConfiguration appDatabaseConfiguration)
    {
        AppointmentStateDatabase = appDatabaseConfiguration ?? throw new ArgumentNullException(nameof(appDatabaseConfiguration));
    }
    public SqlDatabaseConfiguration AppointmentStateDatabase { get; set; }
}