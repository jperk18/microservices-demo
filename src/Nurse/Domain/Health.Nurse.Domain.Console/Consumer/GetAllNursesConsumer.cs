using Health.Nurse.Domain.Console.Core.Exceptions;
using Health.Nurse.Domain.Console.Core.Exceptions.Helpers;
using Health.Nurse.Domain.Console.Queries.GetAllNursesQuery;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class GetAllNursesConsumer : IConsumer<Workflow.Shared.Processes.GetAllNursesQuery>
{
    private readonly IMediator _mediator;

    public GetAllNursesConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<Workflow.Shared.Processes.GetAllNursesQuery> context)
    {
        try
        {
            var r = await _mediator.SendAsync(new GetAllNursesQuery());
            await context.RespondAsync(r.Select(result => new Workflow.Shared.Processes.Core.Models.Nurse()
            {
                FirstName = result.FirstName, LastName = result.LastName, Id = result.Id
            }).ToArray());
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