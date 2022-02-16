using Health.Patient.Domain.Console.Core.Exceptions;
using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Patient.Domain.Console.Queries.GetPatientQuery;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class GetPatientConsumer : IConsumer<Workflow.Shared.Processes.GetPatientQuery>
{
    private readonly IMediator _mediator;

    public GetPatientConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<Workflow.Shared.Processes.GetPatientQuery> context)
    {
        try
        {
            var result = await _mediator.SendAsync(new GetPatientQuery(context.Message.Id));
            await context.RespondAsync(new Workflow.Shared.Processes.Core.Models.Patient()
            {
                DateOfBirth = result.DateOfBirth, FirstName = result.FirstName, LastName = result.LastName, PatientId = result.Id
            });
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