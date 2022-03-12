using Health.Appointment.Domain.Storage.Sql.Core.Configuration;

namespace Health.Appointment.Domain.Console.Core.Configuration;

public class AppointmentDomainConfiguration : IAppointmentDomainConfiguration
{
    public AppointmentDomainConfiguration(IAppointmentStorageConfiguration storageConfiguration, IBrokerCredentialsConfiguration brokerCredentials)
    {
        StorageConfiguration = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }

    public IAppointmentStorageConfiguration StorageConfiguration { get; }
    public IBrokerCredentialsConfiguration BrokerCredentials { get; }
}