using Health.Nurse.Domain.Storage.Sql.Core.Repository.NurseDb;

namespace Health.Nurse.Domain.Storage.Sql;

public interface IUnitOfWork : IDisposable
{
    INurseRepository Nurses { get; }
    Task<int> Complete();
}