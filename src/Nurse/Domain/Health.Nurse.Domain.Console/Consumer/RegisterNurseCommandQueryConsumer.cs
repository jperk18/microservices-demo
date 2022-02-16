using Health.Nurse.Domain.Console.Commands.CreateNurseCommand;
using Health.Nurse.Domain.Console.Core.Exceptions;
using Health.Nurse.Domain.Console.Core.Exceptions.Helpers;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Workflow.Shared.Processes;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class RegisterNurseCommandQueryConsumer : IConsumer<RegisterNurseCommandQuery>
{
    private readonly IMediator _mediator;

    public RegisterNurseCommandQueryConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<RegisterNurseCommandQuery> context)
    {
        try
        {
            var result =
                await _mediator.SendAsync(new CreateNurseCommand(context.Message.FirstName, context.Message.LastName, context.Message.DateOfBirth));
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