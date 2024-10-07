using Domain.Repositories;
using Infra.DataBase.Postgres.Context;

namespace Infra.DataBase.Postgres.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MotorcycleRentalDbContext _context;

        public UnitOfWork(MotorcycleRentalDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
