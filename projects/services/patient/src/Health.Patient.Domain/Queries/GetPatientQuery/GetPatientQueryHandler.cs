using Health.Patient.Domain.Core.Decorators;
using Health.Patient.Domain.Queries.Core;

namespace Health.Patient.Domain.Queries.GetPatientQuery;

[AuditLogPipeline]
[ValidationPipeline]
public sealed class GetPatientQueryHandler : IQueryHandler<GetPatientQuery, string>
{
    public async Task<string> Handle(GetPatientQuery command)
    {
        //TODO: More Business logic
        //TODO: Get Value from db
        
        //Return patient information
        return await Task.FromResult("Patient Information to return");
    }
}