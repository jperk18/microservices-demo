using Health.Patient.Domain.Storage.Sql.Core;
using Health.Patient.Domain.Storage.Sql.Core.Configuration;

namespace Health.Patient.Domain.Console.Core.Configuration;

public class PatientDomainConfiguration : IPatientDomainConfiguration
{
    public PatientDomainConfiguration(IPatientStorageConfiguration storageConfiguration)
    {
        PatientStorageConfiguration = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
    }
    public IPatientStorageConfiguration PatientStorageConfiguration { get; set; }
}