using Health.Shared.Domain.Commands.Core;

namespace Health.Appointment.Domain.Console.Commands.ReferenceData.NurseCreatedCommand;

public sealed class NurseCreatedCommand: ICommand<bool>
{
    public NurseCreatedCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}