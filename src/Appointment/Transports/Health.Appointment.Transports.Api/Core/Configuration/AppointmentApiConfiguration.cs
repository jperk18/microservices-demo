using Health.Shared.Application.Broker.Configuration;

namespace Health.Appointment.Transports.Api.Core.Configuration;

public interface AppointmentApiConfiguration
{
    BrokerCredentialsConfiguration BrokerCredentials { get; }
}

public class AppointmentApiConfigurationDto : AppointmentApiConfiguration
{
    public AppointmentApiConfigurationDto(BrokerCredentialsConfiguration brokerCredentials)
    {
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }

    public BrokerCredentialsConfiguration BrokerCredentials { get; set; }
}