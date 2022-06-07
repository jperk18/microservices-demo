using Health.Appointment.Domain.Console.Commands.ReferenceData.NurseCreatedCommand;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Events;
using MassTransit;

namespace Health.Appointment.Domain.Console.Consumer.ReferenceData;

public class NurseCreatedConsumer : IConsumer<NurseCreated>
{
    private readonly IMediator _mediator;

    public NurseCreatedConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<NurseCreated> context)
    {
        await _mediator.SendAsync(new NurseCreatedCommand(context.Message.Nurse.Id));
    }
}