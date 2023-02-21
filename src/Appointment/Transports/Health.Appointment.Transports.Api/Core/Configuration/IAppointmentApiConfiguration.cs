using Health.Shared.Application.Broker.Configuration;

namespace Health.Appointment.Transports.Api.Core.Configuration;

public interface IAppointmentApiConfiguration
{
    IBrokerCredentialsConfiguration BrokerCredentials { get; }
}