using Health.Appointment.Domain.Console.Exceptions;
using Health.Appointment.Domain.Storage.Sql.Appointment;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Services;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Queries;
using Health.Shared.Workflow.Processes.Sagas.Appointment;
using MassTransit;

namespace Health.Appointment.Domain.Console.Consumers;

public class RequestScheduleAppointmentConsumer : IConsumer<RequestScheduleAppointment>
{
    private readonly IValidationService<RequestScheduleAppointment> _validationService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IRequestClient<GetPatient> _getPatientRequestClient;

    public RequestScheduleAppointmentConsumer(IValidationService<RequestScheduleAppointment> validationService, IAppointmentRepository appointmentRepository, IRequestClient<GetPatient> getPatientRequestClient)
    {
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _getPatientRequestClient = getPatientRequestClient ?? throw new ArgumentNullException(nameof(getPatientRequestClient));
    }
    public async Task Consume(ConsumeContext<RequestScheduleAppointment> context)
    {
        try
        {
            await _validationService.Validate(context.Message);
            
            var (getPatientSuccessResult, getPatientFailedResult) = await _getPatientRequestClient
                .GetResponse<GetPatientSuccess, GetPatientFailed>(new
                {
                    Id = context.Message.PatientId
                });

            if (!getPatientSuccessResult.IsCompletedSuccessfully)
                throw AppointmentDomainExceptions.PatientNotFound(context.Message.PatientId, (RequestScheduleAppointment e) => e.PatientId);

            var patient = (await getPatientSuccessResult).Message.Patient;
            var appointmentId = Guid.NewGuid();
            
            await context.Publish<ScheduleAppointment>(new
            {
                AppointmentId = appointmentId,
                PatientId = patient.Id
            });
                
            await context.RespondAsync<RequestScheduleAppointmentSuccess>(new { AppointmentId = appointmentId });
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