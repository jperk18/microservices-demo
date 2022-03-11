namespace Health.Nurse.Transports.Api.Core.Configuration;

public class NurseApiConfiguration : INurseApiConfiguration
{
    public NurseApiConfiguration(IBrokerCredentialsConfiguration brokerCredentials)
    {
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }

    public IBrokerCredentialsConfiguration BrokerCredentials { get; set; }
}