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

            builder
                .HasOne(f => f.Student)
                .WithMany(s => s.FinalPerformanceAccountingRecords)
                .HasForeignKey(f => f.StudentId)
                .IsRequired();

            builder
                .HasOne(f => f.Journal)
                .WithMany(j => j.FinalPerformanceAccounting)
                .HasForeignKey(f => f.JournalId)
                .IsRequired();
        }
    }
}
