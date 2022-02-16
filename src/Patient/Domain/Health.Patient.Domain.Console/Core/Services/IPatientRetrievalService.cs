namespace Health.Patient.Domain.Console.Core.Services;

public interface IPatientRetrievalService
{
    Task<bool> PatientExists(Guid patientId);
}