namespace Health.Patient.Transports.Api.Core.Configuration;

public interface IBrokerCredentialsConfiguration
{
    string? Host { get; }
    string? Username { get; }
    string? Password { get; }
}