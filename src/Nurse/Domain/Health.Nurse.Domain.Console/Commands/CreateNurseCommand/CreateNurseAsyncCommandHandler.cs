using Health.Nurse.Domain.Console.Core.Decorators;
using Health.Nurse.Domain.Console.Core.Models;
using Health.Nurse.Domain.Console.Core.Pipelines;
using Health.Nurse.Domain.Storage.Sql;
using Health.Shared.Domain.Commands.Core;
using Health.Shared.Domain.Core.Decorators;

namespace Health.Nurse.Domain.Console.Commands.CreateNurseCommand;

[LoggingPipeline]
[ExceptionPipeline]
[ValidationPipeline]
[NurseTransactionPipeline]
public sealed class CreateNurseAsyncCommandHandler : IAsyncCommandHandler<Commands.CreateNurseCommand.CreateNurseCommand, NurseRecord>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateNurseAsyncCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<NurseRecord> Handle(Commands.CreateNurseCommand.CreateNurseCommand command)
    {
        //TODO: More Business logic
        var p = await _unitOfWork.Nurses.Add(new Nurse.Domain.Storage.Sql.Core.Databases.NurseDb.Models.Nurse(
            Guid.NewGuid(), command.FirstName, command.LastName, command.DateOfBirth
        ));

        await _unitOfWork.Complete();
        return new NurseRecord(p.FirstName, p.LastName, p.DateOfBirth, p.Id);
    }
}