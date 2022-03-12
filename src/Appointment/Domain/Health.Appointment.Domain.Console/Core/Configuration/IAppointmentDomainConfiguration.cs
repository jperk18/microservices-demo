using Health.Appointment.Domain.Storage.Sql.Core.Configuration;

namespace Health.Appointment.Domain.Console.Core.Configuration;

public interface IAppointmentDomainConfiguration
{
    IAppointmentStorageConfiguration StorageConfiguration { get; }
    IBrokerCredentialsConfiguration BrokerCredentials { get; }
}