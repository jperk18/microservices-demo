using Health.Appointment.Domain.Console.Exceptions;
using Health.Appointment.Domain.Console.Services;
using Health.Appointment.Domain.Storage.Sql.Appointment;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Services;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Sagas.Appointment;
using MassTransit;

namespace Health.Appointment.Domain.Console.Consumers;

public class RequestPatientCheckInConsumer : IConsumer<RequestPatientCheckIn>
{
    private readonly IAppointmentValidationService<RequestPatientCheckIn> _validator;
    private readonly IAppointmentRepository _appointmentRepository;

    public RequestPatientCheckInConsumer(IAppointmentValidationService<RequestPatientCheckIn> validator, IAppointmentRepository appointmentRepository)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
    }
    public async Task Consume(ConsumeContext<RequestPatientCheckIn> context)
    {
        try
        {
            //Validate
            await _validator.Validate(context.Message);
            
            var appointmentId = context.Message.AppointmentId;
            var appointment = await _appointmentRepository.AppointmentState.GetById(appointmentId) ?? throw AppointmentDomainExceptions.AppointmentNotExist(appointmentId, (RequestPatientCheckIn e) => e.AppointmentId);

            var scheduledPatients = await _appointmentRepository.GetScheduledAppointments();

            if (scheduledPatients == null || !scheduledPatients.Contains(appointmentId))
                throw AppointmentDomainExceptions.ScheduledAppointmentNotFound(appointmentId,
                    (RequestPatientCheckIn e) => e.AppointmentId);
            
            await context.Publish<PatientCheckedIn>(new {AppointmentId = appointment.CorrelationId});
            await context.RespondAsync<RequestPatientCheckInSuccess>(new { AppointmentId = appointment.CorrelationId });
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