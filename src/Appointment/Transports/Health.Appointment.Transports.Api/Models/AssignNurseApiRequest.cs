namespace Health.Appointment.Transports.Api.Models;

public interface AssignNurseApiRequest
{
    Guid Appointment { get; }
    Guid Nurse { get; }
}