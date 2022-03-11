namespace Health.Patient.Domain.Console.Core.Configuration;

public interface IBrokerCredentialsConfiguration
{
    string? Host { get; }
    string? Username { get; }
    string? Password { get; }
}