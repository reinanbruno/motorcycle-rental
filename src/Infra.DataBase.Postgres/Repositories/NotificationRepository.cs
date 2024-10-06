using Domain.Repositories;
using Infra.DataBase.Postgres.Context;

namespace Infra.DataBase.Postgres.Repositories
{
    public class NotificationRepository : BaseRepository<Domain.Entities.Notification>, INotificationRepository
    {
        public NotificationRepository(MotorcycleRentalDbContext context) : base(context)
        {
        }
    }
}
