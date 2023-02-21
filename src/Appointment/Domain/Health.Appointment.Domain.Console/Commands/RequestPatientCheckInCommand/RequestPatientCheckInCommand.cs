using Health.Shared.Domain.Mediator.Commands;

namespace Health.Appointment.Domain.Console.Commands.RequestPatientCheckInCommand;

public sealed class RequestPatientCheckInCommand: ICommand<Guid>
{
    public RequestPatientCheckInCommand(Guid appointment)
    {
        Appointment = appointment;
    }

    public Guid Appointment { get; set; }
}