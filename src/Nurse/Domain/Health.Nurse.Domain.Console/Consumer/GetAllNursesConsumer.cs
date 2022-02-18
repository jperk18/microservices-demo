using Health.Nurse.Domain.Console.Queries.GetAllNursesQuery;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class GetAllNursesConsumer : IConsumer<GetAllNurses>
{
    private readonly IMediator _mediator;

    public GetAllNursesConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<GetAllNurses> context)
    {
        var r = await _mediator.SendAsync(new GetAllNursesQuery());
        await context.RespondAsync<GetAllNursesSuccess>(new
        {
            Nurses = r.Select(result =>
                new Shared.Workflow.Processes.Inner.Models.NurseDto(result.Id, result.FirstName, result.LastName,
                    result.DateOfBirth))
        });
    }
}