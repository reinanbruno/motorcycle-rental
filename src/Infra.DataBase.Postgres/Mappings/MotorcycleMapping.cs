using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.DataBase.Postgres.Mappings
{
    internal class MotorcycleMapping : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
               .Property(e => e.CreationDate)
               .HasColumnType("timestamp without time zone");

            builder
                .Property(e => e.Plate)
                .IsRequired()
                .HasMaxLength(10);

            builder
                .Property(e => e.Model)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(e => e.FabricationYear)
                .IsRequired();

            builder
                .HasIndex(e => e.Plate)
                .IsUnique();
        }
    }
}
