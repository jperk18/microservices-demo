using Health.Appointment.Domain.Console.Queries.GetAllWaitingPatientsQuery;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;

namespace Health.Appointment.Domain.Console.Consumer;

public class GetAllWaitingPatientsConsumer : IConsumer<GetAllWaitingPatients>
{
    private readonly IMediator _mediator;

    public GetAllWaitingPatientsConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<GetAllWaitingPatients> context)
    {
        var r = await _mediator.SendAsync(new GetAllWaitingPatientsQuery());
        var patientRecords = r as Guid[] ?? r.ToArray();

        await context.RespondAsync<GetAllWaitingPatientsSuccess>(new
        {
            Patients = patientRecords
        });
    }
}