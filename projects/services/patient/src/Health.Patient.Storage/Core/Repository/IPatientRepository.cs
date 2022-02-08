using Health.Patient.Storage.Core.Repository.Generic;

namespace Health.Patient.Storage;

public interface IPatientRepository : IGenericRepository<Core.Database.Models.Patient>
{
    
}