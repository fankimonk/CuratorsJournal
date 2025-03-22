using Domain.Entities.JournalContent.FinalPerformanceAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PerformanceAccountingGradesConfiguration : IEntityTypeConfiguration<PerformanceAccountingGrade>
    {
        public void Configure(EntityTypeBuilder<PerformanceAccountingGrade> builder)
        {
            builder.HasKey(f => f.Id);

            builder.ToTable(t => t.HasCheckConstraint("CHK_PAG_Grade", "[Grade] >= 0 and [Grade] <= 10"));

            builder
                .HasOne(f => f.PerformanceAccountingColumn)
                .WithMany(p => p.PerformanceAccountingGrades)
                .HasForeignKey(f => f.PerformanceAccountingColumnId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(f => f.PerformanceAccountingRecord)
                .WithMany(p => p.PerformanceAccountingGrades)
                .HasForeignKey(f => f.PerformanceAccountingRecordId)
                .IsRequired();
        }
    }
}
