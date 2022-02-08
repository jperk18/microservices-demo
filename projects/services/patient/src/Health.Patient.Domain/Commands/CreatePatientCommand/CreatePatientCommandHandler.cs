using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Core.Decorators;
using Health.Patient.Storage;

namespace Health.Patient.Domain.Commands.CreatePatientCommand;

[AuditLogPipeline]
[ValidationPipeline]
[TransactionPipeline]
public sealed class CreatePatientCommandHandler : ICommandHandler<CreatePatientCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreatePatientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<Guid> Handle(CreatePatientCommand command)
    {
        //TODO: More Business logic
        var p = await _unitOfWork.Patients.Add(new Storage.Core.Database.Models.Patient()
        {
            Id = Guid.NewGuid(), FirstName = command.FirstName, LastName = command.LastName, DateOfBirth = command.DateOfBirth
        });

        await _unitOfWork.Complete();
        return p.Id;
    }
}