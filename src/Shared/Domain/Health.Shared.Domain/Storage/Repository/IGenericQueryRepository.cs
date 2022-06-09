using System.Linq.Expressions;

namespace Health.Shared.Domain.Storage.Repository;

public interface IGenericQueryRepository<T> where T : class
{
    Task<T?> GetById(Guid id);
    IEnumerable<T> GetAll();
    IEnumerable<T?> Find(Expression<Func<T, bool>> expression);
}