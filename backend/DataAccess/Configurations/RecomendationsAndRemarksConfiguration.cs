using Domain.Entities.JournalContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class RecomendationsAndRemarksConfiguration : IEntityTypeConfiguration<RecomendationsAndRemarks>
    {
        public void Configure(EntityTypeBuilder<RecomendationsAndRemarks> builder)
        {
            builder.HasKey(rr => rr.Id);

            builder.Property(rr => rr.Content).HasColumnType("nvarchar(max)");
            builder.Property(rr => rr.Result).HasColumnType("nvarchar(max)");

            builder
                .HasOne(rr => rr.Reviewer)
                .WithMany(r => r.RecomendationsAndRemarks)
                .HasForeignKey(rr => rr.ReviewerId)
                .IsRequired();

            builder
                .HasOne(rr => rr.Journal)
                .WithMany(j => j.RecomendationsAndRemarks)
                .HasForeignKey(rr => rr.JournalId)
                .IsRequired();
        }
    }
}
