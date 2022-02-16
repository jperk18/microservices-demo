using Health.Patient.Domain.Storage.Sql.Core.Configuration.Inner;

namespace Health.Patient.Domain.Storage.Sql.Core.Configuration;

public interface IPatientStorageConfiguration
{
    SqlDatabaseConfiguration PatientDatabase { get; set; }
}