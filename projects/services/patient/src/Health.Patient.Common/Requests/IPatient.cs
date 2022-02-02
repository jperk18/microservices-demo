namespace Health.Patient.Common.Requests;

public interface IPatient
{
    Guid PatientId { get; }
    string FirstName { get; }
    string LastName { get; }
}