using Domain.Entities.JournalContent.Pages.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SocioPedagogicalCharacteristicsPageAttributesConfiguration : IEntityTypeConfiguration<SocioPedagogicalCharacteristicsPageAttributes>
    {
        public void Configure(EntityTypeBuilder<SocioPedagogicalCharacteristicsPageAttributes> builder)
        {
            builder.HasKey(h => h.Id);

            builder
                .HasOne(h => h.Page)
                .WithOne(p => p.SocioPedagogicalCharacteristicsPageAttributes)
                .HasForeignKey<SocioPedagogicalCharacteristicsPageAttributes>(h => h.PageId)
                .IsRequired();

            builder
                .HasOne(h => h.AcademicYear)
                .WithMany(a => a.SocioPedagogicalCharacteristicsPageAttributes)
                .HasForeignKey(h => h.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
