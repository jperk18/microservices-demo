using Health.Patient.Domain.Storage.Sql.Core.Configuration;

namespace Health.Patient.Domain.Console.Core.Configuration;

public interface IPatientDomainConfiguration
{
    IPatientStorageConfiguration PatientStorageConfiguration { get; set; }
}