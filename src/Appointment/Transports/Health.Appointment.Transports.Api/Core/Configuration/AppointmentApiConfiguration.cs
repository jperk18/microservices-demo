using Health.Shared.Application.Configuration;

namespace Health.Appointment.Transports.Api.Core.Configuration;

public class AppointmentApiConfiguration : IAppointmentApiConfiguration
{
    public AppointmentApiConfiguration(IBrokerCredentialsConfiguration brokerCredentials)
    {
        BrokerCredentials = brokerCredentials ?? throw new ArgumentNullException(nameof(brokerCredentials));
    }

    public IBrokerCredentialsConfiguration BrokerCredentials { get; set; }
}