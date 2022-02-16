using Health.Patient.Domain.Console.Core.Exceptions.Helpers;
using Health.Shared.Domain.Core.Exceptions;
using Health.Shared.Domain.Mediator;
using Health.Workflow.Shared.Processes;
using Health.Workflow.Shared.Processes.Appointment;
using MassTransit;

namespace Health.Patient.Domain.Console.Consumer;

public class CheckInPatientCommandQueryConsumer : IConsumer<CheckInPatientCommandQuery>
{
    private readonly IMediator _mediator;

    public CheckInPatientCommandQueryConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<CheckInPatientCommandQuery> context)
    {
        try
        {
            var result = await _mediator.SendAsync(new Queries.GetPatientQuery.GetPatientQuery(context.Message.Id));
; 
            await context.Publish<IPatientCheckedIn>(new
            {
                AppointmentId = Guid.NewGuid(),
                Patient = new
                {
                    result.Id,
                    result.FirstName,
                    result.LastName
                },
                Timestamp = InVar.Timestamp
            });

            if (context.RequestId != null)
                await context.RespondAsync<CheckInPatientSuccessResponse>(new {
                    PatientId = result.Id,
                });
        }
        catch (DomainValidationException e)
        {
            var workflowException = e.ToWorkflowValidationObject();
            if (context.RequestId != null)
                await context.RespondAsync<CheckInPatientFailResponse>(new
                {
                    PatientId = context.Message.Id,
                    Error = workflowException
                });
        }
    }
}