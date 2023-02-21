using Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;
using Health.Shared.Application.Broker.Configuration;

namespace Health.Appointment.Domain.Console.Core.Configuration;

public interface AppointmentDomainConfiguration
{
    AppointmentStorageConfiguration AppointmentStorageConfiguration { get; }
    BrokerCredentialsConfiguration BrokerCredentials { get; }
}