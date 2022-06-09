using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Patient.Domain.Console.Queries.GetPatientQuery;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class GetPatientConsumer : IConsumer<GetPatient>
{
    private readonly IMediator _mediator;

    public GetPatientConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<GetPatient> context)
    {
        try
        {
            var result = await _mediator.SendAsync(new GetPatientQuery(context.Message.Id));
            await context.RespondAsync<GetPatientSuccess>(new
            {
                Patient = new
                {
                    result.Id,
                    result.FirstName,
                    result.LastName,
                    result.DateOfBirth
                }
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<GetPatientFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}