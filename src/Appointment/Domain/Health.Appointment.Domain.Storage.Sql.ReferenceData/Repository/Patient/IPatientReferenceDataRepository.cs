using Health.Shared.Domain.Storage.Repository;

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Patient;

public interface IPatientReferenceDataRepository : IGenericRepository<ReferenceData.Database.Entities.Patient>
{
}