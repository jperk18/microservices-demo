namespace Health.Shared.Application.Configuration;

public interface IBrokerCredentialsConfiguration
{
    string? Host { get; }
    string? Username { get; }
    string? Password { get; }
}