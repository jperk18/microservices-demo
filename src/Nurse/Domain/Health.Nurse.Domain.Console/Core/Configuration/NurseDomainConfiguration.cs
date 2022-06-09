using Health.Nurse.Domain.Storage.Sql.Core.Configuration;
using Health.Shared.Application.Configuration;

namespace Health.Nurse.Domain.Console.Core.Configuration;

public class NurseDomainConfiguration : INurseDomainConfiguration
{
    public NurseDomainConfiguration(INurseStorageConfiguration storageConfiguration, IBrokerCredentialsConfiguration brokerCredentials)
    {
        NurseStorage = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }
    public INurseStorageConfiguration NurseStorage { get; }
    public IBrokerCredentialsConfiguration BrokerCredentials { get; }
}