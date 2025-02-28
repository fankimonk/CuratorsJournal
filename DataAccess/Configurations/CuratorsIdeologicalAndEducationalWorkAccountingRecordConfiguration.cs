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

            builder.ToTable(t => t.HasCheckConstraint("CHK_CIAEWA_Month", "[Month] >= 0 and [Month] <= 12"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_CIAEWA_Year", "[Year] >= 0"));

            builder.ToTable(t => t.HasCheckConstraint("CHK_CIAEWA_StartEndDates", "[EndDate] >= [StartDate]"));

            builder.Property(c => c.WorkContent).HasColumnType("nvarchar(max)");

            builder
                .HasOne(c => c.Page)
                .WithMany(p => p.CuratorsIdeologicalAndEducationalWorkAccounting)
                .HasForeignKey(c => c.PageId)
                .IsRequired();
        }
    }
}
