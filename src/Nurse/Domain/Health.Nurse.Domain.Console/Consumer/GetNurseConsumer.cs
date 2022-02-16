using Health.Nurse.Domain.Console.Core.Exceptions;
using Health.Nurse.Domain.Console.Core.Exceptions.Helpers;
using Health.Nurse.Domain.Console.Mediator;
using Health.Nurse.Domain.Console.Queries.GetNurseQuery;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class GetNurseConsumer : IConsumer<Workflow.Shared.Processes.GetNurseQuery>
{
    private readonly IDomainMediator _mediator;

    public GetNurseConsumer(IDomainMediator mediator)
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
            await context.RespondAsync(e.ToValidationObject().ToWorkflowValidationObject());
        }
        
    }
}