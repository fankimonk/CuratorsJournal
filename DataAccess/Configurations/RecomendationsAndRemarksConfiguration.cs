using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class RecomendationsAndRemarksConfiguration : IEntityTypeConfiguration<RecomendationsAndRemarksRecord>
    {
        public void Configure(EntityTypeBuilder<RecomendationsAndRemarksRecord> builder)
        {
            builder.HasKey(rr => rr.Id);

            builder.Property(rr => rr.Content).HasColumnType("nvarchar(max)");
            builder.Property(rr => rr.Result).HasColumnType("nvarchar(max)");

            builder.ToTable(t => t.HasCheckConstraint("CHK_RAR_Dates", "[ExecutionDate] >= [Date]"));

            builder
                .HasOne(rr => rr.Reviewer)
                .WithMany(r => r.RecomendationsAndRemarks)
                .HasForeignKey(rr => rr.ReviewerId);

            builder
                .HasOne(rr => rr.Page)
                .WithMany(p => p.RecomendationsAndRemarks)
                .HasForeignKey(rr => rr.PageId)
                .IsRequired();
        }
    }
}
