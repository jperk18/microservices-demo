using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Core.Decorators;

namespace Health.Patient.Domain.Commands.CreatePatientCommand;

[AuditLogPipeline]
[ValidationPipeline]
public sealed class CreatePatientCommandHandler : ICommandHandler<CreatePatientCommand, Guid>
{
    public async Task<Guid> Handle(CreatePatientCommand command)
    {
        //TODO: More Business logic
        //TODO: Database Save
        
        //Return result identifier
        return await Task.FromResult(Guid.NewGuid());
    }
}