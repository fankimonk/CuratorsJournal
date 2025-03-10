using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class FinalPerformanceAccountingConfiguration : IEntityTypeConfiguration<FinalPerformanceAccountingRecord>
    {
        public void Configure(EntityTypeBuilder<FinalPerformanceAccountingRecord> builder)
        {
            builder.HasKey(f => f.Id);

            builder.ToTable(t => t.HasCheckConstraint("CHK_FPA_Number", "[Number] >= 0"));

            builder
                .HasOne(f => f.Student)
                .WithMany(s => s.FinalPerformanceAccountingRecords)
                .HasForeignKey(f => f.StudentId);

            builder
                .HasOne(f => f.Page)
                .WithMany(p => p.FinalPerformanceAccounting)
                .HasForeignKey(f => f.PageId)
                .IsRequired();
        }
    }
}
