using Health.Appointment.Domain.Console.Commands.RequestPatientCheckInCommand;
using Health.Appointment.Domain.Console.Core.Exceptions.Helpers;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Commands;
using MassTransit;

namespace Health.Appointment.Domain.Console.Consumer;

public class RequestPatientCheckInConsumer : IConsumer<RequestPatientCheckIn>
{
    private readonly IMediator _mediator;

    public RequestPatientCheckInConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<RequestPatientCheckIn> context)
    {
        try
        {
            var result =
                await _mediator.SendAsync(new RequestPatientCheckInCommand(context.Message.AppointmentId));
            await context.RespondAsync<RequestPatientCheckInSuccess>(new { AppointmentId = result });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<RequestPatientCheckInFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}