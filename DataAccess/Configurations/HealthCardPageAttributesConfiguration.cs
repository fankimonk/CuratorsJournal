using Domain.Entities.JournalContent.Pages.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class HealthCardPageAttributesConfiguration : IEntityTypeConfiguration<HealthCardPageAttributes>
    {
        public void Configure(EntityTypeBuilder<HealthCardPageAttributes> builder)
        {
            builder.HasKey(h => h.Id);

            builder
                .HasOne(h => h.Page)
                .WithOne(p => p.HealthCardPageAttributes)
                .HasForeignKey<HealthCardPageAttributes>(h => h.PageId)
                .IsRequired();

            builder
                .HasOne(h => h.AcademicYear)
                .WithMany(a => a.HealthCardPageAttributes)
                .HasForeignKey(h => h.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
