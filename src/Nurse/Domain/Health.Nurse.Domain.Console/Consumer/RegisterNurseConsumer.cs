using Health.Nurse.Domain.Console.Commands.CreateNurseCommand;
using Health.Nurse.Domain.Console.Core.Exceptions.Helpers;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Commands;
using MassTransit;

namespace Health.Nurse.Domain.Console.Consumer;

public class RegisterNurseConsumer : IConsumer<RegisterNurse>
{
    private readonly IMediator _mediator;

    public RegisterNurseConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<RegisterNurse> context)
    {
        try
        {
            var result =
                await _mediator.SendAsync(new CreateNurseCommand(context.Message.FirstName, context.Message.LastName, context.Message.DateOfBirth));
            await context.RespondAsync<RegisterNurseSuccess>(new
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                DateOfBirth = result.DateOfBirth
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<RegisterNurseFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}