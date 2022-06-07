using Health.Shared.Domain.Commands.Core;

namespace Health.Appointment.Domain.Console.Commands.RequestScheduleAppointmentCommand;

public sealed class RequestScheduleAppointmentCommand: ICommand<Guid>
{
    public RequestScheduleAppointmentCommand(Guid patient)
    {
        Patient = patient;
    }

    public Guid Patient { get; set; }
}