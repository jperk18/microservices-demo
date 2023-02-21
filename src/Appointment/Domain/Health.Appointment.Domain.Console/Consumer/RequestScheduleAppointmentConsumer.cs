using Health.Appointment.Domain.Console.Commands.RequestNurseAssignmentForAppointmentCommand;
using Health.Appointment.Domain.Console.Commands.RequestScheduleAppointmentCommand;
using Health.Appointment.Domain.Console.Core.Exceptions.Helpers;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Shared.Workflow.Processes.Commands;
using MassTransit;

namespace Health.Appointment.Domain.Console.Consumer;

public class RequestScheduleAppointmentConsumer : IConsumer<RequestScheduleAppointment>
{
    private readonly IMediator _mediator;

    public RequestScheduleAppointmentConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    public async Task Consume(ConsumeContext<RequestScheduleAppointment> context)
    {
        try
        {
            var result =
                await _mediator.SendAsync(new RequestScheduleAppointmentCommand(context.Message.PatientId));
            await context.RespondAsync<RequestScheduleAppointmentSuccess>(new { AppointmentId = result });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<RequestScheduleAppointmentFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
    }
}