using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.DataBase.Postgres.Mappings
{
    internal class DeliveryPersonMapping : IEntityTypeConfiguration<DeliveryPerson>
    {
        public void Configure(EntityTypeBuilder<DeliveryPerson> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
               .Property(e => e.CreationDate)
               .HasColumnType("timestamp without time zone");

            builder
               .Property(e => e.DateOfBirth)
               .IsRequired()
               .HasColumnType("timestamp without time zone");

            builder
                .Property(e => e.DriverLicenseType)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<DriverLicenseType>(v)
                );

            builder
                .Property(e => e.DriverLicenseNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder
                .Property(e => e.DriverLicenseImage)
                .IsRequired()
                .HasMaxLength(400);

            builder
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(e => e.Document)
                .IsRequired()
                .HasMaxLength(14);

            builder
                .HasIndex(e => e.DriverLicenseNumber)
                .IsUnique();

            builder
                .HasIndex(e => e.Document)
                .IsUnique();
        }
    }
}
