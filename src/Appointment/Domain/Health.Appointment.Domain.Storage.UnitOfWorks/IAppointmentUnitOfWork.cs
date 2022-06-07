using Health.Appointment.Domain.Storage.Sql.Appointment.Repository.AppointmentState;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Nurse;
using Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Patient;

namespace Health.Appointment.Domain.Storage.UnitOfWorks;

public interface IAppointmentUnitOfWork : IDisposable
{
    IAppointmentStateRepository AppointmentState { get; }
    
    IPatientReferenceDataQueryRepository PatientReferenceData { get; }
    INurseReferenceDataQueryRepository NurseReferenceData { get; }
    Task<int> Complete();
}