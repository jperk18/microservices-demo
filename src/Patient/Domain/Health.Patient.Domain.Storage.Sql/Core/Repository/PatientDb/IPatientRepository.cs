using Health.Patient.Domain.Storage.Sql.Core.Repository.Core.Generic;

namespace Health.Patient.Domain.Storage.Sql.Core.Repository.PatientDb;

public interface IPatientRepository : IGenericRepository<Domain.Storage.Sql.Core.Databases.PatientDb.Models.Patient>
{
    
}