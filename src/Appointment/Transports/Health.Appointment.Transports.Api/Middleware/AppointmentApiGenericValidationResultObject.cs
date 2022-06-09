using Health.Shared.Application.Api.Responses;

namespace Health.Appointment.Transports.Api.Middleware;

public class AppointmentApiGenericValidationResultObject : ApiGenericValidationResultObject 
{
    public AppointmentApiGenericValidationResultObject(string title, int status, string detail, IReadOnlyDictionary<string, string[]> errors) : base(title, status, detail, errors) { }
}