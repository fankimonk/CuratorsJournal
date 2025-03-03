using Domain.Entities.JournalContent.Pages.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CuratorsIdeologicalAndEducationalWorkPageAttributesConfiguration 
        : IEntityTypeConfiguration<CuratorsIdeologicalAndEducationalWorkPageAttributes>
    {
        public void Configure(EntityTypeBuilder<CuratorsIdeologicalAndEducationalWorkPageAttributes> builder)
        {
            builder.HasKey(h => h.Id);

            builder.ToTable(t => t.HasCheckConstraint("CHK_CIAEWA_Month", "[Month] >= 0 and [Month] <= 12"));
            builder.ToTable(t => t.HasCheckConstraint("CHK_CIAEWA_Year", "[Year] >= 0"));

            builder
                .HasOne(h => h.Page)
                .WithOne(p => p.CuratorsIdeologicalAndEducationalWorkPageAttributes)
                .HasForeignKey<CuratorsIdeologicalAndEducationalWorkPageAttributes>(h => h.PageId)
                .IsRequired();
        }
    }
}
