using Health.Patient.Domain.Console.Core.Exceptions;
using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Patient.Domain.Console.Queries.GetAllPatientsQuery;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Core.Exceptions.Models;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class GetAllPatientsConsumer : IConsumer<GetAllPatients>
{
    private readonly IMediator _mediator;

    public GetAllPatientsConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<GetAllPatients> context)
    {
        var r = await _mediator.SendAsync(new GetAllPatientsQuery());

        await context.RespondAsync<GetAllPatientsSuccess>(new
        {
            Patients = r.Select(result => new Shared.Workflow.Processes.Inner.Models.PatientDto(
                result.Id, result.FirstName, result.LastName, result.DateOfBirth
            )).ToArray()
        });
    }
}