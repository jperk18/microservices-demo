using Health.Shared.Domain.Commands.Core;

namespace Health.Appointment.Domain.Console.Commands.ReferenceData.PatientCreatedCommand;

public sealed class PatientCreatedCommand: ICommand<bool>
{
    public PatientCreatedCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}