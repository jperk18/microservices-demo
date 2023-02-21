using Health.Appointment.Domain.Console.Core.Exceptions;
using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.Sql.Appointment;
using Health.Shared.Domain.Mediator.Commands;
using Health.Shared.Domain.Mediator.Decorators;
using Health.Shared.Workflow.Processes.Queries;
using Health.Shared.Workflow.Processes.Sagas.Appointment;
using MassTransit;
using MassTransit.Transactions;

namespace Health.Appointment.Domain.Console.Commands.RequestScheduleAppointmentCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[AppointmentTransactionPipeline]
public sealed class RequestScheduleAppointmentCommandHandler : IAsyncCommandHandler<RequestScheduleAppointmentCommand, Guid>
{
    private readonly IAppointmentRepository _unitOfWork;
    private readonly IRequestClient<GetPatient> _getPatientRequestClient;
    private readonly ITransactionalBus _transactionalBus;

    public RequestScheduleAppointmentCommandHandler(IAppointmentRepository unitOfWork, IRequestClient<GetPatient> getPatientRequestClient, ITransactionalBus transactionalBus)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _getPatientRequestClient = getPatientRequestClient ?? throw new ArgumentNullException(nameof(getPatientRequestClient));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
    }
    
    public async Task<Guid> Handle(RequestScheduleAppointmentCommand command)
    {
        var (result, errors) = await _getPatientRequestClient
            .GetResponse<GetPatientSuccess, GetPatientFailed>(new
            {
                Id = command.Patient
            });

        if (!result.IsCompletedSuccessfully)
            throw AppointmentDomainExceptions.PatientNotFound(command.Patient, (RequestScheduleAppointmentCommand e) => e.Patient);

        var patient = (await result).Message.Patient;
        var appointmentId = Guid.NewGuid();
        await _transactionalBus.Publish<ScheduleAppointment>(new
        {
            AppointmentId = appointmentId,
            PatientId = patient.Id
        });
        
        return appointmentId;
    }
}
