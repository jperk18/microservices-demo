namespace Health.Appointment.Transports.Api.Models;

public class AssignNurseApiRequest
{
    public Guid Appointment { get; set; }
    public Guid Nurse { get; set; }
}