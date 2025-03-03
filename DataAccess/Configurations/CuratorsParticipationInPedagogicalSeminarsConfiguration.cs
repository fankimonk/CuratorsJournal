using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CuratorsParticipationInPedagogicalSeminarsConfiguration : IEntityTypeConfiguration<CuratorsParticipationInPedagogicalSeminarsRecord>
    {
        public void Configure(EntityTypeBuilder<CuratorsParticipationInPedagogicalSeminarsRecord> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Topic).HasColumnType("nvarchar(max)");
            builder.Property(c => c.ParticipationForm).HasColumnType("nvarchar(max)");
            builder.Property(c => c.SeminarLocation).HasColumnType("nvarchar(max)");
            builder.Property(c => c.Note).HasColumnType("nvarchar(max)");

            builder
                .HasOne(c => c.Page)
                .WithMany(p => p.CuratorsParticipationInPedagogicalSeminars)
                .HasForeignKey(c => c.PageId)
                .IsRequired();
        }
    }
}
