using Domain.Entities;
using Domain.Repositories;
using Infra.DataBase.Postgres.Context;

namespace Infra.DataBase.Postgres.Repositories
{
    public class DeliveryPersonRepository : BaseRepository<DeliveryPerson>, IDeliveryPersonRepository
    {
        public DeliveryPersonRepository(MotorcycleRentalDbContext context) : base(context)
        {
        }
    }
}
