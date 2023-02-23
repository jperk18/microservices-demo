using Health.Shared.Domain.Storage.Repository;

namespace Health.Nurse.Domain.Storage.Sql;

public interface INurseRepository : IDisposable
{
    IGenericRepository<Core.Databases.NurseDb.Models.Nurse> Nurses { get; }
    Task<int> Complete();
}