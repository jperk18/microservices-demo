namespace Health.Appointment.Transports.Api.Models;

public interface ScheduleAppointmentApiRequest
{
    Guid Patient { get; }
}

public class ScheduleAppointmentApiResponse
{
    public Guid Appointment { get; set; }
}