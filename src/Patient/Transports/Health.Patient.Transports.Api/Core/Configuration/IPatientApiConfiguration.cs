using Health.Shared.Application.Broker.Configuration;

namespace Health.Patient.Transports.Api.Core.Configuration;

public interface IPatientApiConfiguration
{
    IBrokerCredentialsConfiguration BrokerCredentials { get; }
}