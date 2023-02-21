using Health.Appointment.Domain.Console.Core.Exceptions;
using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.Sql.Appointment;
using Health.Shared.Domain.Mediator.Commands;
using Health.Shared.Domain.Mediator.Decorators;
using Health.Shared.Workflow.Processes.Sagas.Appointment;
using MassTransit.Transactions;

namespace Health.Appointment.Domain.Console.Commands.RequestPatientCheckInCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[AppointmentTransactionPipeline]
public sealed class RequestPatientCheckInCommandHandler : IAsyncCommandHandler<RequestPatientCheckInCommand, Guid>
{
    private readonly IAppointmentRepository _unitOfWork;
    private readonly ITransactionalBus _transactionalBus;

    public RequestPatientCheckInCommandHandler(IAppointmentRepository unitOfWork, ITransactionalBus transactionalBus)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
    }
    
    public async Task<Guid> Handle(RequestPatientCheckInCommand command)
    {
        var appointment = await _unitOfWork.AppointmentState.GetById(command.Appointment) ?? throw AppointmentDomainExceptions.AppointmentNotExist(command.Appointment, (RequestPatientCheckInCommand e) => e.Appointment);

        var scheduledPatients = await _unitOfWork.GetScheduledAppointments();

        if (scheduledPatients == null || !scheduledPatients.Contains(command.Appointment))
            throw AppointmentDomainExceptions.ScheduledAppointmentNotFound(command.Appointment,
                (RequestPatientCheckInCommand e) => e.Appointment);
        
        await _transactionalBus.Publish<PatientCheckedIn>(new
        {
            AppointmentId = appointment.CorrelationId
        });
        
        return command.Appointment;
    }
}
