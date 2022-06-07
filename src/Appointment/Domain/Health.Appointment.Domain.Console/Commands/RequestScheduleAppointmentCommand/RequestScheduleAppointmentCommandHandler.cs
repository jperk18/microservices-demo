using Health.Appointment.Domain.Console.Core.Exceptions;
using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.UnitOfWorks;
using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Core.Decorators;
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
    private readonly IAppointmentUnitOfWork _unitOfWork;
    private readonly ITransactionalBus _transactionalBus;

    public RequestScheduleAppointmentCommandHandler(IAppointmentUnitOfWork unitOfWork, ITransactionalBus transactionalBus)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
    }
    
    public async Task<Guid> Handle(RequestScheduleAppointmentCommand command)
    {
        var appointmentId = Guid.NewGuid();
        await _transactionalBus.Publish<ScheduleAppointment>(new
        {
            AppointmentId = appointmentId,
            PatientId = command.Patient
        });
        
        return appointmentId;
    }
}
