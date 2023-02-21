using Health.Shared.Domain.Mediator.Commands;

namespace Health.Appointment.Domain.Console.Commands.ReferenceData.PatientCreatedCommand;

public sealed class PatientCreatedCommand: ICommand<bool>
{
    public PatientCreatedCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}