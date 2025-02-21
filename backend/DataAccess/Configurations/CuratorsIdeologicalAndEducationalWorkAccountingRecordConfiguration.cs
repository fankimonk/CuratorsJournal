using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CuratorsIdeologicalAndEducationalWorkAccountingRecordConfiguration : IEntityTypeConfiguration<CuratorsIdeologicalAndEducationalWorkAccountingRecord>
    {
        public void Configure(EntityTypeBuilder<CuratorsIdeologicalAndEducationalWorkAccountingRecord> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.WorkContent).HasColumnType("nvarchar(max)");

            builder
                .HasOne(c => c.Journal)
                .WithMany(j => j.CuratorsIdeologicalAndEducationalWorkAccounting)
                .HasForeignKey(c => c.JournalId)
                .IsRequired();
        }
    }
}
