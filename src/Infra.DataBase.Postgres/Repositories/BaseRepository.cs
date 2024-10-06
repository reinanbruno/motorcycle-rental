using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Infra.DataBase.Postgres.Context;
using System.Linq.Expressions;

namespace Infra.DataBase.Postgres.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly MotorcycleRentalDbContext _context;

        public BaseRepository(MotorcycleRentalDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<T> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
