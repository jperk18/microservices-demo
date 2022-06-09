using Health.Shared.Domain.Commands.Core;

namespace Health.Appointment.Domain.Console.Commands.RequestNurseAssignmentForAppointmentCommand;

public sealed class RequestNurseAssignmentForAppointmentCommand: ICommand<bool>
{
    public RequestNurseAssignmentForAppointmentCommand(Guid appointment, Guid nurse)
    {
        Appointment = appointment;
        Nurse = nurse;
    }

    public Guid Appointment { get; set; }
    public Guid Nurse { get; set; }
}