using Health.Shared.Domain.Storage.Configuration;

namespace Health.Nurse.Domain.Storage.Sql.Core.Configuration;

public interface NurseStorageConfiguration
{
    SqlDatabaseConfiguration NurseDatabase { get; set; }
}

public class NurseStorageConfigurationDto : NurseStorageConfiguration
{
    public NurseStorageConfigurationDto(SqlDatabaseConfiguration patientDatabaseConfiguration)
    {
        NurseDatabase = patientDatabaseConfiguration ?? throw new ArgumentNullException(nameof(patientDatabaseConfiguration));
    }
    
    public SqlDatabaseConfiguration NurseDatabase { get; set; }
}