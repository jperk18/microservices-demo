using Health.Shared.Domain.Mediator.Commands;

namespace Health.Appointment.Domain.Console.Commands.ReferenceData.NurseCreatedCommand;

public sealed class NurseCreatedCommand: ICommand<bool>
{
    public NurseCreatedCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}