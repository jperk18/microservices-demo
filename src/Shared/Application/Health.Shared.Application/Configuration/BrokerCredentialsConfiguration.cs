namespace Health.Shared.Application.Configuration;

public class BrokerCredentialsConfiguration : IBrokerCredentialsConfiguration
{
    public BrokerCredentialsConfiguration(){}
    public string? Host { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}