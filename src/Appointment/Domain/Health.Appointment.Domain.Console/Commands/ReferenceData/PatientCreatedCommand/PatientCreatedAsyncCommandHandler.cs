using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.UnitOfWorks;
using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Core.Decorators;
using MassTransit.Transactions;

namespace Health.Appointment.Domain.Console.Commands.ReferenceData.PatientCreatedCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[AppointmentTransactionPipeline]
public sealed class PatientCreatedAsyncCommandHandler : IAsyncCommandHandler<PatientCreatedCommand, bool>
{
    private readonly IRefDataUnitOfWork _unitOfWork;
    private readonly ITransactionalBus _transactionalBus;

    public PatientCreatedAsyncCommandHandler(IRefDataUnitOfWork unitOfWork, ITransactionalBus transactionalBus)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
    }

    public async Task<bool> Handle(PatientCreatedCommand command)
    {
        await _unitOfWork.PatientReferenceData.Add(new Storage.Sql.ReferenceData.Database.Entities.Patient(){ PatientId = command.Id });
        await _unitOfWork.Complete();
        return true;
    }
}