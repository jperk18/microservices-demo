using Health.Appointment.Domain.Storage.Sql.Core.Configuration.Inner;

namespace Health.Appointment.Domain.Storage.Sql.Core.Configuration;

public class AppointmentStorageConfiguration : IAppointmentStorageConfiguration
{
    public AppointmentStorageConfiguration(SqlDatabaseConfiguration databaseConfiguration)
    {
        AppointmentStateDatabase = databaseConfiguration ?? throw new ArgumentNullException(nameof(databaseConfiguration));
    }
    
    public SqlDatabaseConfiguration AppointmentStateDatabase { get; set; }
}