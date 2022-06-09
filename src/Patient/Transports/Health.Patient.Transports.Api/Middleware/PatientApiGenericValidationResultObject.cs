using Health.Shared.Application.Api.Responses;

namespace Health.Patient.Transports.Api.Middleware;

public class PatientApiGenericValidationResultObject : ApiGenericValidationResultObject 
{
    public PatientApiGenericValidationResultObject(string title, int status, string detail, IReadOnlyDictionary<string, string[]> errors) : base(title, status, detail, errors) { }
}