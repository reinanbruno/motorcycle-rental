using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.DataBase.Postgres.Mappings
{
    internal class RentalPlanMapping : IEntityTypeConfiguration<RentalPlan>
    {
        public void Configure(EntityTypeBuilder<RentalPlan> builder)
        {
            builder
               .HasKey(e => e.Id);

            builder
               .Property(e => e.CreationDate)
               .HasColumnType("timestamp without time zone");

            builder
                .Property(e => e.DurationDays)
                .IsRequired();

            builder
                .Property(e => e.DailyAmount)
                .IsRequired();

            builder
                .Property(e => e.FinePercentage)
                .IsRequired();

            builder.HasIndex(e => e.DurationDays)
                   .IsUnique();

            builder.HasData(
                new RentalPlan { Id = Guid.NewGuid(), DurationDays = 7, DailyAmount = 30, FinePercentage = 20 },
                new RentalPlan { Id = Guid.NewGuid(), DurationDays = 15, DailyAmount = 28, FinePercentage = 40 },
                new RentalPlan { Id = Guid.NewGuid(), DurationDays = 30, DailyAmount = 22, FinePercentage = 40 },
                new RentalPlan { Id = Guid.NewGuid(), DurationDays = 45, DailyAmount = 20, FinePercentage = 40 },
                new RentalPlan { Id = Guid.NewGuid(), DurationDays = 50, DailyAmount = 18, FinePercentage = 40 }
            );
        }
    }
}
