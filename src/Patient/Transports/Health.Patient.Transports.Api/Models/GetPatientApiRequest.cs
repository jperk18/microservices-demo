using Health.Patient.Transports.Api.Models.Interfaces;

namespace Health.Patient.Transports.Api.Models;

public class GetPatientApiRequest : IPatientIdentifer
{
    public Guid PatientId { get; set; }
}