using Health.Appointment.Domain.Console.Core.Exceptions;
using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.UnitOfWorks;
using Health.Shared.Domain.Mediator.Commands;
using Health.Shared.Domain.Mediator.Decorators;
using Health.Shared.Workflow.Processes.Sagas.Appointment;
using MassTransit.Transactions;

namespace Health.Appointment.Domain.Console.Commands.RequestNurseAssignmentForAppointmentCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[AppointmentTransactionPipeline]
public sealed class RequestNurseAssignmentForAppointmentCommandHandler : IAsyncCommandHandler<RequestNurseAssignmentForAppointmentCommand, bool>
{
    private readonly IAppointmentUnitOfWork _unitOfWork;
    private readonly ITransactionalBus _transactionalBus;

    public RequestNurseAssignmentForAppointmentCommandHandler(IAppointmentUnitOfWork unitOfWork, ITransactionalBus transactionalBus)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
    }
    
    public async Task<bool> Handle(RequestNurseAssignmentForAppointmentCommand command)
    {
        var appointment = (await _unitOfWork.AppointmentState.GetById(command.Appointment)) ?? throw AppointmentDomainExceptions.AppointmentNotExist(command.Appointment, (RequestNurseAssignmentForAppointmentCommand e) => e.Appointment);
        
        
        var nurse = (await _unitOfWork.NurseReferenceData.GetById(command.Nurse)) ?? throw AppointmentDomainExceptions.NurseNotFound(command.Nurse, (RequestNurseAssignmentForAppointmentCommand e) => e.Nurse);
        
        await _unitOfWork.Complete();
        await _transactionalBus.Publish<AssignedNurseForAppointment>(new
        {
            AppointmentId = appointment.CorrelationId,
            NurseId = command.Nurse
        });
        
        return true;
    }
}
