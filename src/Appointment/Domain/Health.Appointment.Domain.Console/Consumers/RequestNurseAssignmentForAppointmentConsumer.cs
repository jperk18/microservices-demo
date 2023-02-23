using Health.Appointment.Domain.Console.Exceptions;
using Health.Appointment.Domain.Storage.Sql.Appointment;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Services;
using Health.Shared.Workflow.Processes.Commands;
using Health.Shared.Workflow.Processes.Queries;
using Health.Shared.Workflow.Processes.Sagas.Appointment;
using MassTransit;

namespace Health.Appointment.Domain.Console.Consumers;

public class RequestNurseAssignmentForAppointmentConsumer : IConsumer<RequestNurseAssignmentForAppointment>
{
    private readonly IValidationService<RequestNurseAssignmentForAppointment> _validationService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IRequestClient<GetNurse> _getNurseRequestClient;

    public RequestNurseAssignmentForAppointmentConsumer(IValidationService<RequestNurseAssignmentForAppointment> validationService, IAppointmentRepository appointmentRepository, IRequestClient<GetNurse> getNurseRequestClient)
    {
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _getNurseRequestClient = getNurseRequestClient ?? throw new ArgumentNullException(nameof(getNurseRequestClient));
    }
    public async Task Consume(ConsumeContext<RequestNurseAssignmentForAppointment> context)
    {
        try
        {
            await _validationService.Validate(context.Message);
            
            var appointment = (await _appointmentRepository.AppointmentState.GetById(context.Message.AppointmentId)) ?? throw AppointmentDomainExceptions.AppointmentNotExist(context.Message.AppointmentId, (RequestNurseAssignmentForAppointment e) => e.AppointmentId);
        
            var (result, errors) = await _getNurseRequestClient
                .GetResponse<GetNurseSuccess, GetNurseFailed>(new
                {
                    Id = context.Message.NurseId
                });

            if (!result.IsCompletedSuccessfully)
                throw AppointmentDomainExceptions.NurseNotFound(context.Message.NurseId, (RequestNurseAssignmentForAppointment e) => e.NurseId);

            await context.Publish<AssignedNurseForAppointment>(new
            {
                AppointmentId = context.Message.AppointmentId,
                NurseId = context.Message.NurseId
            });
                
            await context.RespondAsync<RequestNurseAssignmentForAppointmentSuccess>(new { Assigned = true });
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