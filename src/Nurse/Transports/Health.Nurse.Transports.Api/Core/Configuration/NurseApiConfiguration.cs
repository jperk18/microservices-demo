using Health.Shared.Application.Broker.Configuration;

namespace Health.Nurse.Transports.Api.Core.Configuration;

public interface NurseApiConfiguration
{
    BrokerCredentialsConfiguration BrokerCredentials { get; }
}

public class NurseApiConfigurationDto : NurseApiConfiguration
{
    public NurseApiConfigurationDto(BrokerCredentialsConfiguration brokerCredentials)
    {
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }

    public BrokerCredentialsConfiguration BrokerCredentials { get; set; }
}