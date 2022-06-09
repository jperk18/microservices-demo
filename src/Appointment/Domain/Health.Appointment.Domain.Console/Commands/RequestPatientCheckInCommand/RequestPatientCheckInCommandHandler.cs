using Health.Appointment.Domain.Console.Core.Exceptions;
using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.UnitOfWorks;
using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Core.Decorators;
using Health.Shared.Workflow.Processes.Sagas.Appointment;
using MassTransit.Transactions;

namespace Health.Appointment.Domain.Console.Commands.RequestPatientCheckInCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[AppointmentTransactionPipeline]
public sealed class RequestPatientCheckInCommandHandler : IAsyncCommandHandler<RequestPatientCheckInCommand, Guid>
{
    private readonly IAppointmentUnitOfWork _unitOfWork;
    private readonly ITransactionalBus _transactionalBus;

    public RequestPatientCheckInCommandHandler(IAppointmentUnitOfWork unitOfWork, ITransactionalBus transactionalBus)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
    }
    
    public async Task<Guid> Handle(RequestPatientCheckInCommand command)
    {
        var appointment = await _unitOfWork.AppointmentState.GetById(command.Appointment);

        if (appointment == null)
            throw new AppointmentDomainValidationException($"Appointment: {command.Appointment} does not exist");
        
        var scheduledPatients = await _unitOfWork.AppointmentState.GetScheduledAppointments();

        if(scheduledPatients == null || !scheduledPatients.Contains(command.Appointment))
                throw new AppointmentDomainValidationException($"Appointment: Unable to check in");
        
        await _transactionalBus.Publish<PatientCheckedIn>(new
        {
            AppointmentId = appointment.CorrelationId
        });
        
        return command.Appointment;
    }
}
