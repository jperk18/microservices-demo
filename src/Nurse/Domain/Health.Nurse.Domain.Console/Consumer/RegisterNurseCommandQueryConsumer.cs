using Health.Nurse.Domain.Console.Commands.CreateNurseCommand;
using Health.Nurse.Domain.Console.Core.Exceptions;
using Health.Nurse.Domain.Console.Core.Exceptions.Helpers;
using Health.Nurse.Domain.Console.Mediator;
using Health.Workflow.Shared.Processes;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class RegisterNurseCommandQueryConsumer : IConsumer<RegisterNurseCommandQuery>
{
    private readonly IDomainMediator _mediator;

    public RegisterNurseCommandQueryConsumer(IDomainMediator mediator)
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
            await context.RespondAsync(e.ToValidationObject().ToWorkflowValidationObject());
        }
    }
}