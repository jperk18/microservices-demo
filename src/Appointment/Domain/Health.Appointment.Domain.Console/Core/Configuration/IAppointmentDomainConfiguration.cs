using Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Core.Configuration;
using Health.Shared.Application.Configuration;

namespace Health.Appointment.Domain.Console.Core.Configuration;

public interface IAppointmentDomainConfiguration
{
    IAppointmentStorageConfiguration AppointmentStorageConfiguration { get; }
    IReferenceDataStorageConfiguration ReferenceDataStorageConfiguration { get; }
    IBrokerCredentialsConfiguration BrokerCredentials { get; }
}