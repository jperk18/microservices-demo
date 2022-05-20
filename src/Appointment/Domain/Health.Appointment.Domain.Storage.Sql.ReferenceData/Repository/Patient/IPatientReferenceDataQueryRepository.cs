using Health.Shared.Domain.Storage.Repository;

namespace Health.Appointment.Domain.Storage.Sql.ReferenceData.Repository.Patient;

public interface IPatientReferenceDataQueryRepository : IGenericQueryRepository<ReferenceData.Database.Entities.Patient>
{
}