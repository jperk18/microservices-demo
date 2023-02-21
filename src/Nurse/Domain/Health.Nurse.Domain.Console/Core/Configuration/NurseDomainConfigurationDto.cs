using Health.Nurse.Domain.Storage.Sql.Core.Configuration;
using Health.Shared.Application.Broker.Configuration;

namespace Health.Nurse.Domain.Console.Core.Configuration;

public class NurseDomainConfigurationDto : NurseDomainConfiguration
{
    public NurseDomainConfigurationDto(NurseStorageConfiguration storageConfiguration, BrokerCredentialsConfiguration brokerCredentials)
    {
        NurseStorage = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }
    public NurseStorageConfiguration NurseStorage { get; }
    public BrokerCredentialsConfiguration BrokerCredentials { get; }
}