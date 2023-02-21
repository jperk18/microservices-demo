using Health.Nurse.Domain.Console.Core.Exceptions.Helpers;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using GetNurseQuery = Health.Nurse.Domain.Console.Queries.GetNurseQuery.GetNurseQuery;

namespace Health.Nurse.Domain.Console.Consumer;

public class GetNurseConsumer : IConsumer<GetNurse>
{
    private readonly IMediator _mediator;

    public GetNurseConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<GetNurse> context)
    {
        try
        {
            var result = await _mediator.SendAsync(new GetNurseQuery(context.Message.Id));
            
            await context.RespondAsync<GetNurseSuccess>(new
            {
                Nurse = new
                {
                    Id = result.Id,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    DateOfBirth = result.DateOfBirth
                }
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<GetNurseFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}