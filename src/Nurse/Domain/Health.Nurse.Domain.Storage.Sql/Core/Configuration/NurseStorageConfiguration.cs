using Health.Nurse.Domain.Storage.Sql.Core.Configuration;
using Health.Nurse.Domain.Storage.Sql.Core.Configuration.Inner;

namespace Health.Nurse.Domain.Storage.Sql.Core;

public class NurseStorageConfiguration : INurseStorageConfiguration
{
    public NurseStorageConfiguration(SqlDatabaseConfiguration patientDatabaseConfiguration)
    {
        NurseDatabase = patientDatabaseConfiguration ?? throw new ArgumentNullException(nameof(patientDatabaseConfiguration));
    }
    
    public SqlDatabaseConfiguration NurseDatabase { get; set; }
}