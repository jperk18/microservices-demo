using Health.Nurse.Domain.Storage.Sql.Core.Repository.Core.Generic;

namespace Health.Nurse.Domain.Storage.Sql.Core.Repository.NurseDb;

public interface INurseRepository : IGenericRepository<Nurse.Domain.Storage.Sql.Core.Databases.NurseDb.Models.Nurse>
{
    
}