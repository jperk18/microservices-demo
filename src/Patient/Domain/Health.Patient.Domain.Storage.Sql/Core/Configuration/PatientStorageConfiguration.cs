using Health.Patient.Domain.Storage.Sql.Core.Configuration.Inner;

namespace Health.Patient.Domain.Storage.Sql.Core.Configuration;

public class PatientStorageConfiguration : IPatientStorageConfiguration
{
    public PatientStorageConfiguration(SqlDatabaseConfiguration patientDatabaseConfiguration)
    {
        PatientDatabase = patientDatabaseConfiguration ?? throw new ArgumentNullException(nameof(patientDatabaseConfiguration));
    }
    
    public SqlDatabaseConfiguration PatientDatabase { get; set; }
}