using Health.Shared.Application.Configuration;

namespace Health.Nurse.Transports.Api.Core.Configuration;

public interface INurseApiConfiguration
{
    IBrokerCredentialsConfiguration BrokerCredentials { get; }
}