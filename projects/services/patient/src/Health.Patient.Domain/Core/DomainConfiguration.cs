using Health.Patient.Storage.Core;

namespace Health.Patient.Domain.Core;

public interface IDomainConfiguration
{
    StorageConfiguration StorageConfiguration { get; set; }
}

public class DomainConfiguration : IDomainConfiguration
{
    public StorageConfiguration StorageConfiguration { get; set; }
}