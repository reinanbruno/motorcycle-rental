using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IBaseRepository<T> : IDisposable where T : BaseEntity
    {
        void Update(T entity);
        void Delete(T entity);
        Task CreateAsync(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
    }
}
