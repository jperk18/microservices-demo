using Health.Shared.Application.Configuration;

namespace Health.Appointment.Transports.Api.Core.Configuration;

public interface IAppointmentApiConfiguration
{
    IBrokerCredentialsConfiguration BrokerCredentials { get; }
}