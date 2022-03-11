namespace Health.Patient.Transports.Api.Core.Configuration;

public class BrokerCredentialsConfiguration : IBrokerCredentialsConfiguration
{
    public BrokerCredentialsConfiguration(){}
    public string? Host { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}