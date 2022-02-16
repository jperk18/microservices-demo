using Health.Patient.Domain.Console.Core.Exceptions;
using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Patient.Domain.Console.Queries.GetAllPatientsQuery;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class GetAllPatientsConsumer : IConsumer<Workflow.Shared.Processes.GetAllPatientsQuery>
{
    private readonly IMediator _mediator;

    public GetAllPatientsConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<Workflow.Shared.Processes.GetAllPatientsQuery> context)
    {
        try
        {
            var r = await _mediator.SendAsync(new GetAllPatientsQuery());
            await context.RespondAsync(r.Select(result => new Workflow.Shared.Processes.Core.Models.Patient()
            {
                DateOfBirth = result.DateOfBirth, FirstName = result.FirstName, LastName = result.LastName, PatientId = result.Id
            }).ToArray());
        }
        catch (DomainValidationException e)
        {
            if (e is PatientDomainValidationException exception)
            {
                await context.RespondAsync(exception.ToWorkflowValidationObject());
            }
            else
            {
                await context.RespondAsync(e.ToWorkflowValidationObject());
            }
        }
    }
}