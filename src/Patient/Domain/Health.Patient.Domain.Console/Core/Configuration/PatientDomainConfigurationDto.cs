using Health.Patient.Domain.Storage.Sql.Core.Configuration;
using Health.Shared.Application.Broker.Configuration;

namespace Health.Patient.Domain.Console.Core.Configuration;

public class PatientDomainConfigurationDto : PatientDomainConfiguration
{
    public PatientDomainConfigurationDto(PatientStorageConfiguration storage, BrokerCredentialsConfiguration brokerCredentials)
    {
        PatientStorage = storage ?? throw new ArgumentNullException(nameof(storage));
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }
    public PatientStorageConfiguration PatientStorage { get; set; }
    public BrokerCredentialsConfiguration BrokerCredentials { get; }
}