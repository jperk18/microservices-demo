using Health.Patient.Storage.Core;

namespace Health.Patient.Domain.Core;

public class DomainRegistrationConfiguration
{
    public StorageRegistrationConfiguration? StorageConfiguration { get; set; }
}