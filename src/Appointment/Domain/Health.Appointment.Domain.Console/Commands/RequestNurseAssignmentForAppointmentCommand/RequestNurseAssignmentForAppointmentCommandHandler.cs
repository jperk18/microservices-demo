using Health.Appointment.Domain.Console.Core.Exceptions;
using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.Sql.Appointment;
using Health.Shared.Domain.Mediator.Commands;
using Health.Shared.Domain.Mediator.Decorators;
using Health.Shared.Workflow.Processes.Queries;
using Health.Shared.Workflow.Processes.Sagas.Appointment;
using MassTransit;
using MassTransit.Transactions;

namespace Health.Appointment.Domain.Console.Commands.RequestNurseAssignmentForAppointmentCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[AppointmentTransactionPipeline]
public sealed class RequestNurseAssignmentForAppointmentCommandHandler : IAsyncCommandHandler<RequestNurseAssignmentForAppointmentCommand, bool>
{
    private readonly IAppointmentRepository _unitOfWork;
    private readonly IRequestClient<GetNurse> _getNurseRequestClient;
    private readonly ITransactionalBus _transactionalBus;
    

    public RequestNurseAssignmentForAppointmentCommandHandler(IAppointmentRepository unitOfWork, IRequestClient<GetNurse> getNurseRequestClient, ITransactionalBus transactionalBus)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _getNurseRequestClient = getNurseRequestClient ?? throw new ArgumentNullException(nameof(getNurseRequestClient));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
    }
    
    public async Task<bool> Handle(RequestNurseAssignmentForAppointmentCommand command)
    {
        var appointment = (await _unitOfWork.AppointmentState.GetById(command.Appointment)) ?? throw AppointmentDomainExceptions.AppointmentNotExist(command.Appointment, (RequestNurseAssignmentForAppointmentCommand e) => e.Appointment);
        
        var (result, errors) = await _getNurseRequestClient
            .GetResponse<GetNurseSuccess, GetNurseFailed>(new
            {
                Id = command.Nurse
            });

        if (!result.IsCompletedSuccessfully)
            throw AppointmentDomainExceptions.NurseNotFound(command.Nurse, (RequestNurseAssignmentForAppointmentCommand e) => e.Nurse);

        await _unitOfWork.Complete();
        await _transactionalBus.Publish<AssignedNurseForAppointment>(new
        {
            AppointmentId = appointment.CorrelationId,
            NurseId = command.Nurse
        });
        
        return true;
    }
}
