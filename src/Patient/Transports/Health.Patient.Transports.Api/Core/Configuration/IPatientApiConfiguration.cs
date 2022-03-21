using Health.Shared.Application.Configuration;

namespace Health.Patient.Transports.Api.Core.Configuration;

public interface IPatientApiConfiguration
{
    IBrokerCredentialsConfiguration BrokerCredentials { get; }
}