using Health.Shared.Domain.Storage.Configuration;

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Core.Configuration;

public interface IReferenceDataStorageConfiguration
{
    SqlDatabaseConfiguration ReferenceDataDatabase { get; set; }
}