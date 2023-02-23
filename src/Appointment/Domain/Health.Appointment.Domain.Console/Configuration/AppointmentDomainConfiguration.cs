using Health.Appointment.Domain.Storage.Sql.Appointment.Configuration;
using Health.Shared.Application.Broker.Configuration;

namespace Health.Appointment.Domain.Console.Configuration;

public interface AppointmentDomainConfiguration
{
    AppointmentStorageConfiguration AppointmentStorageConfiguration { get; }
    BrokerCredentialsConfiguration BrokerCredentials { get; }
}