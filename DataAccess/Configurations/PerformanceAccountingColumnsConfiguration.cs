using Domain.Entities.JournalContent.FinalPerformanceAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PerformanceAccountingColumnsConfiguration : IEntityTypeConfiguration<PerformanceAccountingColumn>
    {
        public void Configure(EntityTypeBuilder<PerformanceAccountingColumn> builder)
        {
            builder.HasKey(f => f.Id);

            builder
                .HasOne(f => f.CertificationType)
                .WithMany(p => p.PerformanceAccountingColumns)
                .HasForeignKey(f => f.CertificationTypeId)
                .IsRequired();

            builder
                .HasOne(f => f.Subject)
                .WithMany(p => p.PerformanceAccountingColumns)
                .HasForeignKey(f => f.SubjectId);

            builder
                .HasOne(f => f.Page)
                .WithMany(p => p.PerformanceAccountingColumns)
                .HasForeignKey(f => f.PageId)
                .IsRequired();
        }
    }
}
