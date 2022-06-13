namespace Health.Shared.Domain.Storage.Repository;

public interface IGenericRepository<T> : IGenericQueryRepository<T> where T : class
{
    Task<T> Add(T entity);
    Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);
    Task<T> Update(T entity);
    Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<T> AddOrUpdate(Guid id, T entity);
}