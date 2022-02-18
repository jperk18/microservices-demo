using Health.Patient.Domain.Console.Commands.CreatePatientCommand;
using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Commands;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class RegisterPatientConsumer : IConsumer<RegisterPatient>
{
    private readonly IMediator _mediator;

    public RegisterPatientConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<RegisterPatient> context)
    {
        try
        {
            var result =
                await _mediator.SendAsync(new CreatePatientCommand(context.Message.FirstName, context.Message.LastName, context.Message.DateOfBirth));
            await context.RespondAsync<RegisterPatientSuccess>(new
            {
                PatientId = result.Id,
                result.FirstName,
                result.LastName,
                result.DateOfBirth
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<RegisterPatientFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}