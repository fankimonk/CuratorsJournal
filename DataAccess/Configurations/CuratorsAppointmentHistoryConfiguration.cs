using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CuratorsAppointmentHistoryConfiguration : IEntityTypeConfiguration<CuratorsAppointmentHistoryRecord>
    {
        public void Configure(EntityTypeBuilder<CuratorsAppointmentHistoryRecord> builder)
        {
            builder.HasKey(cah => cah.Id);

            builder
                .HasOne(cah => cah.Curator)
                .WithMany(c => c.CuratorsAppointmentHistory)
                .HasForeignKey(cah => cah.CuratorId)
                .IsRequired();

            builder
                .HasOne(cah => cah.Group)
                .WithMany(g => g.CuratorsAppointmentHistory)
                .HasForeignKey(cah => cah.GroupId)
                .IsRequired();
        }
    }
}
