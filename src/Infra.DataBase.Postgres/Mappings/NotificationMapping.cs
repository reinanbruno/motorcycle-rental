using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.DataBase.Postgres.Mappings
{
    internal class NotificationMapping : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder
               .HasKey(e => e.Id);

            builder
               .Property(e => e.CreationDate)
               .HasColumnType("timestamp without time zone");

            builder
               .Property(e => e.EventType)
               .IsRequired();

            builder
               .Property(e => e.Message)
               .IsRequired();
        }
    }
}
