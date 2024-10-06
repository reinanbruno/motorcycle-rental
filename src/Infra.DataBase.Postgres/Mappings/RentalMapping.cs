using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infra.DataBase.Postgres.Mappings
{
    internal class RentalMapping : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder
               .HasKey(e => e.Id);

            builder
               .Property(e => e.CreationDate)
               .HasColumnType("timestamp without time zone");

            builder
               .Property(e => e.StartDate)
               .IsRequired()
               .HasColumnType("timestamp without time zone");
            
            builder
               .Property(e => e.EndDate)
               .IsRequired()
               .HasColumnType("timestamp without time zone");
            
            builder
               .Property(e => e.ExpectedEndDate)
               .IsRequired()
               .HasColumnType("timestamp without time zone");

            builder
               .Property(e => e.ReturnDate)
               .HasColumnType("timestamp without time zone");

            builder
               .Property(e => e.RentalPlanId)
               .IsRequired();

            builder
               .Property(e => e.MotorcycleId)
               .IsRequired();

            builder
               .Property(e => e.DeliveryPersonId)
               .IsRequired();

            builder
               .HasOne(e => e.RentalPlan)
               .WithMany()
               .HasForeignKey(e => e.RentalPlanId);

            builder
               .HasOne(e => e.Motorcycle)
               .WithMany()
               .HasForeignKey(e => e.MotorcycleId);

            builder
               .HasOne(e => e.DeliveryPerson)
               .WithMany()
               .HasForeignKey(e => e.DeliveryPersonId);
        }
    }
}
