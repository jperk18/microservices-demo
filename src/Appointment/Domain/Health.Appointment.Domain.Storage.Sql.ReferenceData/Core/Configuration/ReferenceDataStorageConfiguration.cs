using Health.Shared.Domain.Storage.Configuration;

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Core.Configuration;

public class ReferenceDataStorageConfiguration : IReferenceDataStorageConfiguration
{
    public ReferenceDataStorageConfiguration(SqlDatabaseConfiguration databaseConfiguration)
    {
        ReferenceDataDatabase = databaseConfiguration ?? throw new ArgumentNullException(nameof(databaseConfiguration));
    }
    public SqlDatabaseConfiguration ReferenceDataDatabase { get; set; }
}