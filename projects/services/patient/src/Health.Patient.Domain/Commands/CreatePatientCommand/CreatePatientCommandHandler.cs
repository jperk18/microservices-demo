using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Core.Decorators;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Storage.Sql;

namespace Health.Patient.Domain.Commands.CreatePatientCommand;

[AuditLogPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[TransactionPipeline]
public sealed class CreatePatientCommandHandler : ICommandHandler<CreatePatientCommand, PatientRecord>
{
    private readonly IPatientUnitOfWork _unitOfWork;
    public CreatePatientCommandHandler(IPatientUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<PatientRecord> Handle(CreatePatientCommand command)
    {
        //TODO: More Business logic
        var p = await _unitOfWork.Patients.Add(new Storage.Sql.Core.Databases.PatientDb.Models.Patient(
            Guid.NewGuid(), command.FirstName, command.LastName, command.DateOfBirth
        ));

        await _unitOfWork.Complete();
        return new PatientRecord(p.FirstName, p.LastName, p.DateOfBirth, p.Id);
    }
}