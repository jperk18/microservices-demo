using Health.Appointment.Domain.Console.Commands.RequestNurseAssignmentForAppointmentCommand;
using Health.Appointment.Domain.Console.Core.Exceptions;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Commands;
using MassTransit;

namespace Health.Appointment.Domain.Console.Consumer;

public class RequestNurseAssignmentForAppointmentConsumer : IConsumer<RequestNurseAssignmentForAppointment>
{
    private readonly IMediator _mediator;

    public RequestNurseAssignmentForAppointmentConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<RequestNurseAssignmentForAppointment> context)
    {
        try
        {
            var result =
                await _mediator.SendAsync(new RequestNurseAssignmentForAppointmentCommand(context.Message.AppointmentId, context.Message.NurseId));
            await context.RespondAsync<RequestNurseAssignmentForAppointmentSuccess>(new { Assigned = result });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<RequestNurseAssignmentForAppointmentFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}