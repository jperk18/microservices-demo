using Health.Patient.Transports.Api.Models.Interfaces;

namespace Health.Patient.Transports.Api.Models;

public class CreatePatientApiResponse : IPatientIdentifer
{
    public Guid PatientId { get; set; }
}