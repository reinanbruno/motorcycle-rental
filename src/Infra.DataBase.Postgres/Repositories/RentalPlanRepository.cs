using Domain.Entities;
using Domain.Repositories;
using Infra.DataBase.Postgres.Context;

namespace Infra.DataBase.Postgres.Repositories
{
    public class RentalPlanRepository : BaseRepository<RentalPlan>, IRentalPlanRepository
    {
        public RentalPlanRepository(MotorcycleRentalDbContext context) : base(context)
        {
        }
    }
}
