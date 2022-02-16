using Health.Patient.Domain.Console.Commands.CreatePatientCommand;
using Health.Patient.Domain.Console.Core.Exceptions;
using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Workflow.Shared.Processes;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class RegisterPatientCommandQueryConsumer : IConsumer<RegisterPatientCommandQuery>
{
    private readonly IMediator _mediator;

    public RegisterPatientCommandQueryConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<RegisterPatientCommandQuery> context)
    {
        try
        {
            var result =
                await _mediator.SendAsync(new CreatePatientCommand(context.Message.FirstName, context.Message.LastName, context.Message.DateOfBirth));
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