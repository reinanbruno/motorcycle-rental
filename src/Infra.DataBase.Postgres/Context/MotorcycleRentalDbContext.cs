using Infra.DataBase.Postgres.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infra.DataBase.Postgres.Context
{
    public class MotorcycleRentalDbContext : DbContext
    {
        public MotorcycleRentalDbContext(DbContextOptions<MotorcycleRentalDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotorcycleMapping());
            modelBuilder.ApplyConfiguration(new DeliveryPersonMapping());
            modelBuilder.ApplyConfiguration(new RentalPlanMapping());
            modelBuilder.ApplyConfiguration(new RentalMapping());
            modelBuilder.ApplyConfiguration(new NotificationMapping());
        }
    }
}
