using Domain.Entities;
using Domain.Repositories;
using Infra.DataBase.Postgres.Context;

namespace Infra.DataBase.Postgres.Repositories
{
    public class RentalRepository : BaseRepository<Rental>, IRentalRepository
    {
        public RentalRepository(MotorcycleRentalDbContext context) : base(context)
        {
        }
    }
}
