using Health.Appointment.Domain.Storage.Sql.Appointment.Configuration;
using Health.Shared.Application.Broker.Configuration;

namespace Health.Appointment.Domain.Console.Configuration;

public class AppointmentDomainConfigurationDto : AppointmentDomainConfiguration
{
    public AppointmentDomainConfigurationDto(AppointmentStorageConfiguration storageConfiguration, BrokerCredentialsConfiguration brokerCredentials)
    {
        AppointmentStorageConfiguration = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }

    public AppointmentStorageConfiguration AppointmentStorageConfiguration { get; }
    public BrokerCredentialsConfiguration BrokerCredentials { get; }
}