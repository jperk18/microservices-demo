using Health.Appointment.Domain.Console.Core.Pipelines;
using Health.Appointment.Domain.Storage.UnitOfWorks;
using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Core.Decorators;
using MassTransit.Transactions;

namespace Health.Appointment.Domain.Console.Commands.ReferenceData.NurseCreatedCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[AppointmentTransactionPipeline]
public sealed class NurseCreatedAsyncCommandHandler : IAsyncCommandHandler<NurseCreatedCommand, bool>
{
    private readonly IRefDataUnitOfWork _unitOfWork;
    private readonly ITransactionalBus _transactionalBus;

    public NurseCreatedAsyncCommandHandler(IRefDataUnitOfWork unitOfWork, ITransactionalBus transactionalBus)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
    }

    public async Task<bool> Handle(NurseCreatedCommand command)
    {
        await _unitOfWork.NurseReferenceData.Add(new Storage.Sql.ReferenceData.Database.Entities.Nurse(){ NurseId = command.Id });
        await _unitOfWork.Complete();
        return true;
    }
}