using Health.Nurse.Domain.Storage.Sql.Core.Configuration.Inner;

namespace Health.Nurse.Domain.Storage.Sql.Core.Configuration;

public interface INurseStorageConfiguration
{
    SqlDatabaseConfiguration NurseDatabase { get; set; }
}