using Health.Appointment.Domain.Storage.Sql.Appointment.Core.Configuration;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Core.Configuration;
using Health.Shared.Application.Configuration;

namespace Health.Appointment.Domain.Console.Core.Configuration;

public class AppointmentDomainConfiguration : IAppointmentDomainConfiguration
{
    public AppointmentDomainConfiguration(IAppointmentStorageConfiguration storageConfiguration, IReferenceDataStorageConfiguration appointmentReferenceDataStorageConfiguration, IBrokerCredentialsConfiguration brokerCredentials)
    {
        AppointmentStorageConfiguration = storageConfiguration ?? throw new ArgumentNullException(nameof(storageConfiguration));
        ReferenceDataStorageConfiguration = appointmentReferenceDataStorageConfiguration ?? throw new ArgumentNullException(nameof(appointmentReferenceDataStorageConfiguration));
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }

    public IAppointmentStorageConfiguration AppointmentStorageConfiguration { get; }
    public IReferenceDataStorageConfiguration ReferenceDataStorageConfiguration { get; }
    public IBrokerCredentialsConfiguration BrokerCredentials { get; }
}