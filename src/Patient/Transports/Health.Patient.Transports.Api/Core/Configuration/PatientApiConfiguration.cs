using Health.Shared.Application.Broker.Configuration;

namespace Health.Patient.Transports.Api.Core.Configuration;

public class PatientApiConfiguration : IPatientApiConfiguration
{
    public PatientApiConfiguration(IBrokerCredentialsConfiguration brokerCredentials)
    {
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }

    public IBrokerCredentialsConfiguration BrokerCredentials { get; set; }
}