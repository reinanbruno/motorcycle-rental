using Domain.Entities;
using Domain.Repositories;
using Infra.DataBase.Postgres.Context;

namespace Infra.DataBase.Postgres.Repositories
{
    public class MotorcycleRepository : BaseRepository<Motorcycle>, IMotorcycleRepository
    {
        public MotorcycleRepository(MotorcycleRentalDbContext context) : base(context)
        {
        }
    }
}
