using Health.Shared.Application.Api.Responses;

namespace Health.Nurse.Transports.Api.Middleware;

public class NurseApiGenericValidationResultObject : ApiGenericValidationResultObject
{
    public NurseApiGenericValidationResultObject(string title, int status, string detail, IReadOnlyDictionary<string, string[]> errors) : base(title, status, detail, errors) { }
}