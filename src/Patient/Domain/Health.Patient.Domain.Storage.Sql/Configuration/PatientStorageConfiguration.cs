using Health.Shared.Domain.Storage.Configuration;

namespace Health.Patient.Domain.Storage.Sql.Configuration;

public interface PatientStorageConfiguration
{
    SqlDatabaseConfiguration PatientDatabase { get; set; }
}

public class PatientStorageConfigurationDto : PatientStorageConfiguration
{
    public PatientStorageConfigurationDto(SqlDatabaseConfiguration patientDatabaseConfiguration)
    {
        PatientDatabase = patientDatabaseConfiguration ?? throw new ArgumentNullException(nameof(patientDatabaseConfiguration));
    }
    
    public SqlDatabaseConfiguration PatientDatabase { get; set; }
}