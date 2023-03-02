using Health.Appointment.Domain.Console.Exceptions;
using Health.Appointment.Domain.Console.Services;
using Health.Appointment.Domain.Storage.Sql.Appointment;
using Health.Shared.Domain.Exceptions;
using Health.Shared.Domain.Services;
using Health.Shared.Workflow.Processes.Queries;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Health.Appointment.Domain.Console.Consumers;

public class GetAllWaitingPatientsConsumer : IConsumer<GetAllWaitingPatients>
{
    private readonly IAppointmentValidationService<GetAllWaitingPatients> _validationService;
    private readonly IAppointmentRepository _appointmentRepository;

    public GetAllWaitingPatientsConsumer(ILogger<GetAllWaitingPatientsConsumer> logger, IAppointmentValidationService<GetAllWaitingPatients> validationService, IAppointmentRepository appointmentRepository)
    {
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
    }

    public async Task Consume(ConsumeContext<GetAllWaitingPatients> context)
    {
        try
        {
            await _validationService.Validate(context.Message);
            
            var r = await _appointmentRepository.GetWaitingPatients();
            var patientRecords = r == null ? Array.Empty<Guid>() : r.ToArray();

            await context.RespondAsync<GetAllWaitingPatientsSuccess>(new
            {
                Patients = patientRecords
            });
        }
        catch (DomainValidationException e)
        {
            await context.RespondAsync<GetAllWaitingPatientsFailed>(new
            {
                Error = e.ToWorkflowValidationObject()
            });
        }
        
    }
}