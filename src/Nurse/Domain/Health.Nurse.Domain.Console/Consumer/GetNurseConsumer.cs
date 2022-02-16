using Health.Nurse.Domain.Console.Core.Exceptions;
using Health.Nurse.Domain.Console.Core.Exceptions.Helpers;
using Health.Nurse.Domain.Console.Queries.GetNurseQuery;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class GetNurseConsumer : IConsumer<Workflow.Shared.Processes.GetNurseQuery>
{
    private readonly IMediator _mediator;

    public GetNurseConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<Workflow.Shared.Processes.GetNurseQuery> context)
    {
        try
        {
            var result = await _mediator.SendAsync(new GetNurseQuery(context.Message.Id));
            await context.RespondAsync(new Workflow.Shared.Processes.Core.Models.Nurse()
            {
                FirstName = result.FirstName, LastName = result.LastName, Id = result.Id
            });
        }
        catch (DomainValidationException e)
        {
            if (e is NurseDomainValidationException exception)
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