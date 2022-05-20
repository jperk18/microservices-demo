using Health.Appointment.Domain.Console.Commands.ReferenceData.PatientCreatedCommand;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Events;
using MassTransit;

namespace Health.Appointment.Domain.Console.Consumer.ReferenceData;

public class PatientCreatedConsumer : IConsumer<PatientCreated>
{
    private readonly IMediator _mediator;

    public PatientCreatedConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<PatientCreated> context)
    {
        await _mediator.SendAsync(new PatientCreatedCommand(context.Message.Patient.Id));
    }
}