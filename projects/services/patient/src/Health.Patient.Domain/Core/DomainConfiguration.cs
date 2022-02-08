using Health.Patient.Storage.Core;

namespace Health.Patient.Domain.Core;

public interface IDomainConfiguration
{
    IStorageConfiguration StorageConfiguration { get; set; }
}

public class DomainConfiguration : IDomainConfiguration
{
    public IStorageConfiguration StorageConfiguration { get; set; }
}