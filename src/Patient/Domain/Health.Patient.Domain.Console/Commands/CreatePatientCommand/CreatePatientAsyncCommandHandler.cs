using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Patient.Domain.Console.Core.Models;
using Health.Patient.Domain.Console.Core.Pipelines;
using Health.Patient.Domain.Storage.Sql;
using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Core.Decorators;
using Health.Shared.Workflow.Processes;
using Health.Shared.Workflow.Processes.Events;
using MassTransit.Transactions;

namespace Health.Patient.Domain.Console.Commands.CreatePatientCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[PatientTransactionPipeline]
public sealed class CreatePatientAsyncCommandHandler : IAsyncCommandHandler<Console.Commands.CreatePatientCommand.CreatePatientCommand, PatientRecord>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    private readonly ITransactionalBus _transactionalBus;

    public CreatePatientAsyncCommandHandler(IPatientUnitOfWork unitOfWork, ITransactionalBus transactionalBus)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _transactionalBus = transactionalBus ?? throw new ArgumentNullException(nameof(transactionalBus));
    }
    
    public async Task<PatientRecord> Handle(Console.Commands.CreatePatientCommand.CreatePatientCommand command)
    {
        //TODO: More Business logic
        var p = await _unitOfWork.Patients.Add(new Storage.Sql.Core.Databases.PatientDb.Models.Patient(
            Guid.NewGuid(), command.FirstName, command.LastName, command.DateOfBirth
        ));

        await _unitOfWork.Complete();
        await _transactionalBus.Publish<PatientCreated>(new
        {
            Patient = new
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth
            }
        });
        
        return new PatientRecord(p.FirstName, p.LastName, p.DateOfBirth, p.Id);
    }
}