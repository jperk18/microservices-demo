using Health.Shared.Application.Broker.Configuration;

namespace Health.Patient.Transports.Api.Core.Configuration;

public interface PatientApiConfiguration
{
    BrokerCredentialsConfiguration BrokerCredentials { get; }
}

public class PatientApiConfigurationDto : PatientApiConfiguration
{
    public PatientApiConfigurationDto(BrokerCredentialsConfiguration brokerCredentials)
    {
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }

    public BrokerCredentialsConfiguration BrokerCredentials { get; set; }
}