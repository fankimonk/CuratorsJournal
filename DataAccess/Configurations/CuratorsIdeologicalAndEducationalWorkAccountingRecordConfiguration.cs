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

            builder.ToTable(t => t.HasCheckConstraint("CHK_CIAEWA_StartEndDays", "[EndDay] >= [StartDay]"));

            builder.Property(c => c.WorkContent).HasColumnType("nvarchar(max)");

            builder
                .HasOne(c => c.Page)
                .WithMany(p => p.CuratorsIdeologicalAndEducationalWorkAccounting)
                .HasForeignKey(c => c.PageId)
                .IsRequired();
        }
    }
}
