namespace Health.Appointment.Transports.Api.Models;

public class ScheduleAppointmentApiRequest
{
    public Guid Patient { get; set; }
}

public class ScheduleAppointmentApiResponse
{
    public Guid Appointment { get; set; }
}