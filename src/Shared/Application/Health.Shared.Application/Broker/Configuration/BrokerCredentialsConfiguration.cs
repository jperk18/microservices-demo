namespace Health.Shared.Application.Broker.Configuration;

public interface BrokerCredentialsConfiguration
{
    string? Host { get; }
    string? Username { get; }
    string? Password { get; }
}

public class BrokerCredentialsConfigurationDto : BrokerCredentialsConfiguration
{
    public BrokerCredentialsConfigurationDto(){}
    public string? Host { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}